using UnityEngine;

public class Logger {

    private readonly string _title;
    private readonly Object _context;
    public bool Enabled;
    
    public Logger(Object context, bool enabled = true) {
        _context = context;
        Enabled = enabled;

        _title = context.GetType().Name;
    }

    public void Log(string msg) {
        if (Enabled)
            Debug.Log(FormatMsg(msg), _context);
    }

    public void Warn(string msg) {
        if(Enabled)
            Debug.LogWarning(FormatMsg(msg), _context);
    }

    public void Err(string msg) {
        if(Enabled)
            Debug.LogError(FormatMsg(msg), _context);
    }

    private string FormatMsg(string msg) {
        return "<b>" + _title + "</b>: " + msg;
    }
    
}
