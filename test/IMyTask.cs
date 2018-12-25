?using System;

namespace MyThreadPool
{
    /// <summary>
    /// ��������� �����, �������� � ���������� � ���� �������. 
    /// ������������� ����������� ������ ��������� ������ � �� ��������� - 
    /// ��������� ��� ��� ���.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public interface IMyTask<TResult>
    {
        bool IsCompleted { get; }
        TResult Result { get; }
        IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func);
    }
}