?using System;
using System.Collections.Generic;
using System.Threading;

namespace MyThreadPool
{
    /// <summary>
    /// ����� ��������� ��� ����� � ������������� ����������� 
    /// �������. ������ ���� ��������� ���������� ������, ����
    /// ��������� � ������ ��������, ���� ��������� ����� ���.
    /// </summary>
    public class MyThreadPool
    {
        private int threadsNumber;
        private Thread[] threads;
        private Queue<Action> tasks;

        private Object lockObject = new Object();
        private CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        private CancellationToken token;

        private AutoResetEvent readyTask;

        /// <summary>
        /// ����������� ������ ������� ��������� ���������� �������,
        /// ������ ����� �� ����������� � ��������� � ����� ��������
        /// </summary>
        /// <param name="n">���������� ������� � ����</param>
        public MyThreadPool(int n)
        {
    