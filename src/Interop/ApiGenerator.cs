using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;

namespace QuicNet.Interop
{
    public static unsafe class ApiGenerator
    {

        private static void GenerateConstructorIl(ILGenerator generator, FieldInfo apiField, FieldInfo closeField)
        {
            generator.Emit(OpCodes.Ldarg_0); // Load this
            generator.Emit(OpCodes.Call, typeof(object).GetConstructor(Array.Empty<Type>())); // call objects constructor
            generator.Emit(OpCodes.Ldarg_0); // Load this
            generator.Emit(OpCodes.Ldarg_1); // Load api table argument
            generator.Emit(OpCodes.Stfld, apiField);
            generator.Emit(OpCodes.Ldarg_0); // Load this
            generator.Emit(OpCodes.Ldarg_2); // Load closer
            generator.Emit(OpCodes.Stfld, closeField);
            generator.Emit(OpCodes.Ret);
        }

        private static void GenerateDisposeIl(ILGenerator generator, FieldInfo apiField, FieldInfo closeField)
        {
            GC.KeepAlive(apiField);
            generator.Emit(OpCodes.Ldarg_0); // Load this
            generator.Emit(OpCodes.Ldfld, closeField); // load close field
            generator.Emit(OpCodes.Ldarg_0); // Load this
            generator.Emit(OpCodes.Ldfld, apiField); // Load API
            generator.Emit(OpCodes.Callvirt, closeField.FieldType.GetMethod("Invoke"));
            generator.Emit(OpCodes.Ret);
        }

        private static void GenerateMethodIl(ILGenerator generator, FieldInfo apiField, string fieldName, Type[] parameters, Type returnType, EmitCalliDelegate emitCalli)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                generator.Emit(OpCodes.Ldarg, i + 1); // Load parameter, +1 from static
            }

            generator.Emit(OpCodes.Ldarg_0); // Load this
            generator.Emit(OpCodes.Ldfld, apiField); // Load address of field
            generator.Emit(OpCodes.Ldfld, typeof(NativeQuicApi).GetField(fieldName)); // Load function pointer
            emitCalli(generator, OpCodes.Calli, CallingConvention.Cdecl, returnType, parameters); // call function pointer
            generator.Emit(OpCodes.Ret);
        }

        private static void GenerateCheckedMethodIl(ILGenerator generator, FieldInfo apiField, string fieldName, Type[] parameters, EmitCalliDelegate emitCalli)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                generator.Emit(OpCodes.Ldarg, i + 1); // Load parameter, +1 from static
            }

            generator.Emit(OpCodes.Ldarg_0); // Load this
            generator.Emit(OpCodes.Ldfld, apiField); // Load address of field
            generator.Emit(OpCodes.Ldfld, typeof(NativeQuicApi).GetField(fieldName)); // Load function pointer
            emitCalli(generator, OpCodes.Calli, CallingConvention.Cdecl, typeof(int), parameters); // call function pointer

            var local = generator.DeclareLocal(typeof(int));
            generator.Emit(OpCodes.Stloc_0);
            generator.Emit(OpCodes.Ldloc_0);

            generator.Emit(OpCodes.Ldc_I4_0);
            var label = generator.DefineLabel();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // generate check for failures = < 0
                generator.Emit(OpCodes.Bge, label);
            }
            else
            {
                // generate check for failures = > 0
                generator.Emit(OpCodes.Ble, label);
            }
            generator.Emit(OpCodes.Ldloc_0);
            generator.Emit(OpCodes.Call, typeof(Helpers).GetMethod(nameof(Helpers.CheckException)));
            generator.MarkLabel(label);
            generator.Emit(OpCodes.Ret);
        }

        private delegate void EmitCalliDelegate(ILGenerator generator, OpCode code, CallingConvention convention, Type returnType, Type[] parameterTypes);

        private static ConstructorInfo LocalCreateApiImplementation()
        {
            var emitMethod = typeof(ILGenerator).GetMethod(nameof(ILGenerator.EmitCalli), new Type[] { typeof(OpCode), typeof(CallingConvention), typeof(Type), typeof(Type[]) });

#pragma warning disable CA1303 // Do not pass literals as localized parameters
            var tmp = emitMethod?.CreateDelegate(typeof(EmitCalliDelegate)) ?? throw new PlatformNotSupportedException("This platform does not support calli IL Generation");
#pragma warning restore CA1303 // Do not pass literals as localized parameters
            var emitCalli = (EmitCalliDelegate)tmp;

            var asmBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("QuicApiAsm"), AssemblyBuilderAccess.Run);
            var moduleBuilder = asmBuilder.DefineDynamicModule("QuicApiModule");

            var typeBuilder = moduleBuilder.DefineType("QuicInteropApi");
            typeBuilder.AddInterfaceImplementation(typeof(IDisposable));
            typeBuilder.AddInterfaceImplementation(typeof(IQuicInteropApi));

            var apiField = typeBuilder.DefineField("quicApi", typeof(NativeQuicApi*), FieldAttributes.Private | FieldAttributes.InitOnly);
            var closeField = typeBuilder.DefineField("quicClose", typeof(QuicCloseDelegate), FieldAttributes.Private | FieldAttributes.InitOnly);

            // Define constructor
            var constructor = typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.HasThis, new Type[] { typeof(NativeQuicApi*), typeof(QuicCloseDelegate) });

            GenerateConstructorIl(constructor.GetILGenerator(), apiField, closeField);

            var dispose = typeBuilder.DefineMethod("Dispose", MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Public);

            GenerateDisposeIl(dispose.GetILGenerator(), apiField, closeField);

            foreach (var method in typeof(IQuicInteropApi).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                var parameters = method.GetParameters().Select(x => x.ParameterType).ToArray();
                var methodBuilder = typeBuilder.DefineMethod(method.Name, MethodAttributes.Virtual | MethodAttributes.Public, method.ReturnType, parameters);
                methodBuilder.SetImplementationFlags(MethodImplAttributes.AggressiveInlining);

                if (method.GetCustomAttribute<CheckResultAttribute>() != null)
                {
                    GenerateCheckedMethodIl(methodBuilder.GetILGenerator(), apiField, method.Name, parameters, emitCalli);
                }
                else
                {
                    GenerateMethodIl(methodBuilder.GetILGenerator(), apiField, method.Name, parameters, method.ReturnType, emitCalli);
                }


            }



            var typeInfo = typeBuilder.CreateTypeInfo();
            return typeInfo!.GetConstructors()[0];

        }

        private static Lazy<ConstructorInfo> s_apiConstructor = new Lazy<ConstructorInfo>(() => LocalCreateApiImplementation());

        public unsafe delegate void QuicCloseDelegate(NativeQuicApi* api);

        // This API will be replaced with Function Pointers once they exist, but for now IL generation is by far the
        // fastest way to generate the QUIC api
        public unsafe static IQuicInteropApi CreateApiImplementation(NativeQuicApi* api, QuicCloseDelegate closeDelegate)
        {
            var constructor = s_apiConstructor.Value;
            return (IQuicInteropApi)constructor.Invoke(new object[] { Pointer.Box(api, typeof(NativeQuicApi*)), closeDelegate });
        }
    }
}
