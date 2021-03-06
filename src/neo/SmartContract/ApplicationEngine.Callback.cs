using Neo.SmartContract.Callbacks;
using Neo.VM.Types;
using System;
using Array = Neo.VM.Types.Array;

namespace Neo.SmartContract
{
    partial class ApplicationEngine
    {
        public static readonly InteropDescriptor System_Callback_Create = Register("System.Callback.Create", nameof(CreateCallback), 0_00000400, TriggerType.All, CallFlags.None, false);
        public static readonly InteropDescriptor System_Callback_CreateFromMethod = Register("System.Callback.CreateFromMethod", nameof(CreateCallbackFromMethod), 0_01000000, TriggerType.All, CallFlags.None, false);
        public static readonly InteropDescriptor System_Callback_CreateFromSyscall = Register("System.Callback.CreateFromSyscall", nameof(CreateCallbackFromSyscall), 0_00000400, TriggerType.All, CallFlags.None, false);
        public static readonly InteropDescriptor System_Callback_Invoke = Register("System.Callback.Invoke", nameof(InvokeCallback), 0_01000000, TriggerType.All, CallFlags.None, false);

        internal void InvokeCallback(CallbackBase callback, Array args)
        {
            if (args.Count != callback.ParametersCount)
                throw new InvalidOperationException();
            callback.LoadContext(this, args);
            if (callback is SyscallCallback syscallCallback)
                OnSysCall(syscallCallback.Method);
        }

        internal PointerCallback CreateCallback(Pointer pointer, int parcount)
        {
            return new PointerCallback(CurrentContext, pointer, parcount);
        }

        internal MethodCallback CreateCallbackFromMethod(UInt160 hash, string method)
        {
            return new MethodCallback(this, hash, method);
        }

        internal SyscallCallback CreateCallbackFromSyscall(uint method)
        {
            return new SyscallCallback(method);
        }
    }
}
