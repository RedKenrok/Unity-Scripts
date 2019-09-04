using System;
using System.Diagnostics;

public class Log {
	#region Message
		[Conditional("LOGGING_ENABLED")]
		public static void Message(object message) {
			UnityEngine.Debug.Log(message);
		}
		[Conditional("LOGGING_ENABLED")]
		public static void Message(UnityEngine.Object context, object message) {
			UnityEngine.Debug.Log(message, context);
		}
		[Conditional("LOGGING_ENABLED")]
		public static void MessageFormat(string format, params object[] args) {
			UnityEngine.Debug.LogFormat(format, args);
		}
		[Conditional("LOGGING_ENABLED")]
		public static void MessageFormat(UnityEngine.Object context, string format, params object[] args) {
			UnityEngine.Debug.LogFormat(context, format, args);
		}
	#endregion
	
	#region Warning
		[Conditional("LOGGING_ENABLED")]
		public static void Warning(object message) {
			UnityEngine.Debug.LogWarning(message);
		}
		[Conditional("LOGGING_ENABLED")]
		public static void Warning(UnityEngine.Object context, object message) {
			UnityEngine.Debug.LogWarning(message, context);
		}
		[Conditional("LOGGING_ENABLED")]
		public static void WarningFormat(string format, params object[] args) {
			UnityEngine.Debug.LogWarningFormat(format, args);
		}
		[Conditional("LOGGING_ENABLED")]
		public static void WarningFormat(UnityEngine.Object context, string format, params object[] args) {
			UnityEngine.Debug.LogWarningFormat(context, format, args);
		}
	#endregion
	
	#region Error
		[Conditional("LOGGING_ENABLED")]
		public static void Error(object message) {
			UnityEngine.Debug.LogError(message);
		}
		[Conditional("LOGGING_ENABLED")]
		public static void Error(UnityEngine.Object context, object message) {
			UnityEngine.Debug.LogError(message, context);
		}
		[Conditional("LOGGING_ENABLED")]
		public static void ErrorFormat(string format, params object[] args) {
			UnityEngine.Debug.LogErrorFormat(format, args);
		}
		[Conditional("LOGGING_ENABLED")]
		public static void ErrorFormat(UnityEngine.Object context, string format, params object[] args) {
			UnityEngine.Debug.LogErrorFormat(context, format, args);
		}
	#endregion
	
	#region Assertion
		[Conditional("LOGGING_ENABLED")]
		public static void Assertion(object message) {
			UnityEngine.Debug.LogAssertion(message);
		}
		[Conditional("LOGGING_ENABLED")]
		public static void Assertion(UnityEngine.Object context, object message) {
			UnityEngine.Debug.LogAssertion(message, context);
		}
	#endregion
	
	#region Exception
		[Conditional("LOGGING_ENABLED")]
		public static void Exception(Exception exception) {
			UnityEngine.Debug.LogException(exception);
		}
		[Conditional("LOGGING_ENABLED")]
		public static void Exception(UnityEngine.Object context, Exception exception) {
			UnityEngine.Debug.LogException(exception, context);
		}
	#endregion
}