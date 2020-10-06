using Graphyte.Build.Toolsets.Msvc.Compiler;
using Graphyte.Build.Toolsets.Msvc.Linker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graphyte.Build.Toolsets
{
    public class MsvcOptionsDispatcher
    {
        public delegate void OptionHandler(StringBuilder command, object o);

        private static readonly Dictionary<Type, OptionHandler> s_Options = new Dictionary<Type, OptionHandler>()
        {
            { typeof(DisableSpecificWarnings), HandleDisableSpecificWarnings },
            { typeof(InstructionSet), HandleInstructionSet },
            { typeof(RuntimeTypeInfo), HandleRuntimeTypeInfo },
            { typeof(DelayLoadDlls), HandleDelayLoadDlls },
            { typeof(RuntimeLibrary), HandleRuntimeLibrary },
            { typeof(Subsystem), HandleSubsystem },
        };

        private static void HandleDisableSpecificWarnings(StringBuilder command, object o)
        {
            var data = (DisableSpecificWarnings)o;

            foreach (var warning in data.Values)
            {
                command.Append($@" /wd{warning}");
            }
        }

        private static void HandleInstructionSet(StringBuilder command, object o)
        {
            var data = (InstructionSet)o;

            switch (data)
            {
                case InstructionSet.None:
                case InstructionSet.Disable:
                    break;
                case InstructionSet.NEON:
                    command.Append(" /arch:VFPv4");
                    break;
                case InstructionSet.AVX:
                    command.Append(" /arch:AVX");
                    break;
                case InstructionSet.AVX2:
                    command.Append(" /arch:AVX2");
                    break;
                case InstructionSet.AVX512:
                    command.Append(" /arch:AVX512");
                    break;
            }
        }

        private static void HandleRuntimeTypeInfo(StringBuilder command, object o)
        {
            var data = (RuntimeTypeInfo)o;

            switch (data)
            {
                case RuntimeTypeInfo.Enable:
                    command.Append(" /GR");
                    break;
                case RuntimeTypeInfo.Disable:
                    command.Append(" /GR-");
                    break;
            }
        }

        private static void HandleDelayLoadDlls(StringBuilder command, object o)
        {
            var data = (DelayLoadDlls)o;

            foreach (var item in data.Values)
            {
                command.Append($@" /DELAYLOAD:""{item}""");
            }
        }

        private static void HandleRuntimeLibrary(StringBuilder command, object o)
        {
            var data = (RuntimeLibrary)o;

            switch (data)
            {
                case RuntimeLibrary.MultiThreaded:
                    command.Append(" /MT");
                    break;
                case RuntimeLibrary.MultiThreadedDebug:
                    command.Append(" /MTd");
                    break;
                case RuntimeLibrary.MultiThreadedDLL:
                    command.Append(" /MD");
                    break;
                case RuntimeLibrary.MultiThreadedDebugDLL:
                    command.Append(" /MDd");
                    break;
            }
        }

        private static void HandleSubsystem(StringBuilder command, object value)
        {
            var data = (Subsystem)value;

            switch (data)
            {
                case Subsystem.Console:
                    command.Append(" /SUBSYSTEM:CONSOLE");
                    return;

                case Subsystem.Application:
                    command.Append(" /SUBSYSTEM:WINDOWS");
                    return;

                case Subsystem.Native:
                    command.Append(" /SUBSYSTEM:NATIVE");
                    return;
            }

            throw new ArgumentOutOfRangeException(nameof(value));
        }

        public static void HandleOption(StringBuilder command, object o)
        {
            if (s_Options.TryGetValue(o.GetType(), out var handler))
            {
                handler(command, o);
            }
            else
            {
                throw new ConfigurationFailedException($@"Failed to handle toolchain option {o} of type {o.GetType().Name}");
            }
        }

        public static void HandleOptions(StringBuilder command, IEnumerable<object> options)
        {
            foreach (var option in options)
            {
                HandleOption(command, option);
            }
        }

        public struct OptionAction
        {
            public object Value;
            public Action Action;

            public OptionAction(object value, Action action)
            {
                this.Value = value;
                this.Action = action;
            }
        }

        public static bool Handle(List<object> options, params OptionAction[] actions)
        {
            var type = null as Type;

            foreach (var action in actions)
            {
                if (type == null)
                {
                    type = action.Value.GetType();
                }
                else if (type != action.Value.GetType())
                {
                    throw new Exception();
                }
            }

            if (type == null)
            {
                throw new Exception();
            }

            foreach (var option in options)
            {
                if (option.GetType() == type)
                {
                    foreach (var action in actions)
                    {
                        if (action.Value.Equals(option))
                        {
                            action.Action();
                            return true;
                        }
                    }
                }
            }

            throw new Exception();
        }

        public static void Experimental(StringBuilder command, List<object> options)
        {
            Handle(options,
                new OptionAction(Subsystem.Application, () => command.Append(" /SUBSYSTEM:WINDOWS")),
                new OptionAction(Subsystem.Console, () => command.Append(" /SUBSYSTEM:CONSOLE")),
                new OptionAction(Subsystem.Native, () => command.Append(" /SUBSYSTEM:NATIVE"))
                );
        }
    }
}
