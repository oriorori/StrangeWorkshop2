using System;
using UnityEngine;

public interface IObserver<T>
{
    public void OnNext(T value);
    public void OnError(Exception error);
    public void OnCompleted();
}