using System;
using System.Transactions;

namespace AvaloniaApplication4.Models;


public class MainWIndowModel
{
    private Type _mode_tp;

    public MainWIndowModel(Type mode_tp)
    {
        _mode_tp = mode_tp;
    }
}