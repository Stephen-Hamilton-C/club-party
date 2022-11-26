using UnityEngine;

/// <summary>
/// A simple toggleable logger that prefixes every log message with the name of the class for easy filtering
/// </summary>
public class Logger {

    /// <summary>
    /// The name of the class this logger belongs to
    /// </summary>
    private readonly string _title;
    /// <summary>
    /// The context to use for each log message
    /// </summary>
    private readonly Object _context;
    /// <summary>
    /// Should info logs be logged?
    /// </summary>
    public bool Enabled;
    
    /// <summary>
    /// Creates a new Logger
    /// </summary>
    /// <param name="context">The class this logger is for</param>
    /// <param name="enabled">Whether info logs should be logged or not</param>
    public Logger(Object context, bool enabled = true) {
        _context = context;
        Enabled = enabled;

        _title = context.GetType().Name;
        Log("Hello, world!");
    }

    public void Log(string msg) {
        if (Enabled)
            Debug.Log(FormatMsg(msg), _context);
    }

    public void Warn(string msg) {
        Debug.LogWarning(FormatMsg(msg), _context);
    }

    public void Err(string msg) {
        Debug.LogError(FormatMsg(msg), _context);
    }

    /// <summary>
    /// Prefixes the message with the class name
    /// </summary>
    /// <returns>A formatted message</returns>
    private string FormatMsg(string msg) {
        return "<b>" + _title + "</b>: " + msg;
    }
    
}
