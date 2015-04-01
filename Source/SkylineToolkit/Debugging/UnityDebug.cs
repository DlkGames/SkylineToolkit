using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkylineToolkit.Debugging
{
    public static class UnityDebug
    {
        private static bool isEnabled;

        public static bool IsEnabled
        {
            get { return isEnabled; }
        }

        private static RedirectCallsState logMethod;
        private static RedirectCallsState logFormatMethod;
        private static RedirectCallsState logErrorMethod;
        private static RedirectCallsState logErrorFormatMethod;
        private static RedirectCallsState logWarningMethod;
        private static RedirectCallsState logWarningFormatMethod;
        private static RedirectCallsState logExceptionMethod;

        public static void Enable()
        {
            if (isEnabled)
            {
                return;
            }

            try
            {
                HookMethods();
            }
            catch (Exception ex)
            {
                Log.Warning("Unable to hook into Unity's debug methods. Log entries created using unity Debug.Log* methods won't get caught by SkylineToolkit.");
                Log.Exception(ex);

                isEnabled = false;
            }

            Log.Info("Hooked into Unity's debug methods. Redirecting all messages logged by unity Debug.Log* methods to SkylineToolkit.");
            isEnabled = true;
        }

        public static void Disable()
        {
            if (!isEnabled)
            {
                return;
            }

            UnhookMethods();

            Log.Info("Removed redirection hooks from Unity's debug methods.");
            isEnabled = false;
        }

        private static void HookMethods()
        {
            logMethod = RedirectionHelper.RedirectCalls(
                typeof(UnityEngine.Debug).GetMethod("Log", new[] { typeof(object) }),
                typeof(UnityDebug).GetMethod("LogHook", new[] { typeof(object) }));

            logFormatMethod = RedirectionHelper.RedirectCalls(
                typeof(UnityEngine.Debug).GetMethod("LogFormat", new[] { typeof(string), typeof(object[]) }),
                typeof(UnityDebug).GetMethod("LogFormatHook", new[] { typeof(string), typeof(object[]) }));

            logErrorMethod = RedirectionHelper.RedirectCalls(
                typeof(UnityEngine.Debug).GetMethod("LogError", new[] { typeof(object) }),
                typeof(UnityDebug).GetMethod("LogErrorHook", new[] { typeof(object) }));

            logErrorFormatMethod = RedirectionHelper.RedirectCalls(
                typeof(UnityEngine.Debug).GetMethod("LogErrorFormat", new[] { typeof(string), typeof(object[]) }),
                typeof(UnityDebug).GetMethod("LogErrorFormatHook", new[] { typeof(string), typeof(object[]) }));

            logWarningMethod = RedirectionHelper.RedirectCalls(
                typeof(UnityEngine.Debug).GetMethod("LogWarning", new[] { typeof(object) }),
                typeof(UnityDebug).GetMethod("LogWarningHook", new[] { typeof(object) }));

            logWarningFormatMethod = RedirectionHelper.RedirectCalls(
                typeof(UnityEngine.Debug).GetMethod("LogWarningFormat", new[] { typeof(string), typeof(object[]) }),
                typeof(UnityDebug).GetMethod("LogWarningFormatHook", new[] { typeof(string), typeof(object[]) }));

            logExceptionMethod = RedirectionHelper.RedirectCalls(
                typeof(UnityEngine.Debug).GetMethod("LogException", new[] { typeof(Exception) }),
                typeof(UnityDebug).GetMethod("LogExceptionHook", new[] { typeof(Exception) }));
        }

        private static void UnhookMethods()
        {
            RedirectionHelper.RevertRedirect(
                typeof(UnityEngine.Debug).GetMethod("Log", new[] { typeof(object) }), logMethod);
            RedirectionHelper.RevertRedirect(
                typeof(UnityEngine.Debug).GetMethod("LogFormat", new[] { typeof(string), typeof(object[]) }), logFormatMethod);
            RedirectionHelper.RevertRedirect(
                typeof(UnityEngine.Debug).GetMethod("LogError", new[] { typeof(object) }), logErrorMethod);
            RedirectionHelper.RevertRedirect(
                typeof(UnityEngine.Debug).GetMethod("LogErrorFormat", new[] { typeof(string), typeof(object[]) }), logErrorFormatMethod);
            RedirectionHelper.RevertRedirect(
                typeof(UnityEngine.Debug).GetMethod("LogWarning", new[] { typeof(object) }), logWarningMethod);
            RedirectionHelper.RevertRedirect(
                typeof(UnityEngine.Debug).GetMethod("LogWarningFormat", new[] { typeof(string), typeof(object[]) }), logWarningFormatMethod);
            RedirectionHelper.RevertRedirect(
                typeof(UnityEngine.Debug).GetMethod("LogException", new[] { typeof(Exception) }), logExceptionMethod);
        }

        public static void LogHookHook(object obj)
        {
            Log.Info("UnityEngine.Debug", obj);
        }

        public static void LogFormatHook(string format, params object[] args)
        {
            if (format == null)
            {
                return;
            }

            Log.Info("UnityEngine.Debug", format, args);
        }

        public static void LogErrorHook(object obj)
        {
            Log.Error("UnityEngine.Debug", obj);
        }

        public static void LogErrorFormatHook(string format, params object[] args)
        {
            if (format == null)
            {
                return;
            }

            Log.Error("UnityEngine.Debug", format, args);
        }

        public static void LogWarningHook(object obj)
        {
            Log.Warning("UnityEngine.Debug", obj);
        }

        public static void LogWarningFormatHook(string format, params object[] args)
        {
            if (format == null)
            {
                return;
            }

            Log.Warning("UnityEngine.Debug", format, args);
        }

        public static void LogExceptionHook(Exception exception)
        {
            if (exception == null)
            {
                return;
            }

            Log.Exception("UnityEngine.Debug", exception);
        }
    }

}
