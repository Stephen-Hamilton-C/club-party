using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Logger {

    private readonly string _title;
    private readonly Object _context;
    public bool enabled;
    
    public Logger(Object context, bool enabled = true) {
        _context = context;
        this.enabled = enabled;

        _title = context.GetType().Name;
    }

    public void Log(string msg) {
        if (enabled)
            Debug.Log(FormatMsg(msg), _context);
    }

    public void Warn(string msg) {
        if(enabled)
            Debug.LogWarning(FormatMsg(msg), _context);
    }

    public void Err(string msg) {
        if(enabled)
            Debug.LogError(FormatMsg(msg), _context);
    }

    private string FormatMsg(string msg) {
        return "<b>" + _title + "</b>: " + msg;
    }
    
}
