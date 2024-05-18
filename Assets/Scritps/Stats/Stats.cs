using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sử dụng abstract để các class nào kế thừa thì phải ghi đè phương thức
public abstract class Stats : ScriptableObject
{
    public abstract void Save();
    public abstract void Load();
	public abstract void Upgrade(Action OnSuccess = null, Action OnFailed = null);
    public abstract bool IsMaxLevel();
}
