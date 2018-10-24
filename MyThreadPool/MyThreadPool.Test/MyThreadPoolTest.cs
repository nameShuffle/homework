﻿using System;
using System.Linq;
using Xunit;
using MyThreadPool;
using System.Threading;

namespace MyThreadPool.Test
{
    public class MyThreadPoolTest
    {
        private Random random = new Random();

        [Fact]
        public void NullFunctionTest()
        {
            bool? NullFunction() => null;

            MyThreadPool pool = new MyThreadPool(5);
            var nullTask = pool.AddTask(NullFunction);
            
            Assert.Null(nullTask.Result);
        }

        [Fact]
        public void OneTaskTest()
        {
            int SimpleFunction() => 21;

            MyThreadPool pool = new MyThreadPool(5);
            var nullTask = pool.AddTask(SimpleFunction);

            Assert.Equal(21, nullTask.Result);
        }

        [Fact]
        public void ThreadNumberTest()
        {
            MyThreadPool pool = new MyThreadPool(5);
            Assert.Equal(5, pool.threadsCount());
        }

        [Fact]
        public void IsCompletedTest()
        {
            int SleepFunction()
            { 
                Thread.Sleep(500);
                return 25;
            }

            MyThreadPool pool = new MyThreadPool(5);
            var nullTask = pool.AddTask(SleepFunction);

            Assert.Equal(false, nullTask.IsCompleted);
        }

        [Fact]
        public void MoreThenThreadsTest()
        {
            int FuncOne() => 1;
            int FuncTwo() => 2;
            int FuncThree() => 3;

            MyThreadPool pool = new MyThreadPool(2);
            var taskOne = pool.AddTask(FuncOne);
            var taskTwo = pool.AddTask(FuncTwo);
            var taskThree = pool.AddTask(FuncThree);

            Assert.Equal(1, taskOne.Result);
            Assert.Equal(2, taskTwo.Result);
            Assert.Equal(3, taskThree.Result);
        }

        [Fact]
        public void ContinueWithTest()
        {
            int BasicFunc() => 10;
            int Add(int a)
            {
                return a + 10;
            }

            MyThreadPool pool = new MyThreadPool(4);
            var taskOne = pool.AddTask(BasicFunc);
            var taskTwo = taskOne.ContinueWith(Add);

            Assert.Equal(10, taskOne.Result);
            Assert.Equal(20, taskTwo.Result);
        }

        [Fact]
        public void DoubleContinueWithTest()
        {
            int BasicFunc() => 10;
            int AddOne(int a)
            {
                return a + 10;
            }
            int AddTwo(int a)
            {
                return a + 11;
            }

            MyThreadPool pool = new MyThreadPool(4);
            var taskOne = pool.AddTask(BasicFunc);
            var taskTwo = taskOne.ContinueWith(AddOne);
            var taskThree = taskOne.ContinueWith(AddTwo);

            Assert.Equal(10, taskOne.Result);
            Assert.Equal(20, taskTwo.Result);
            Assert.Equal(21, taskThree.Result);
        }

        [Fact]
        public void CansellationTest()
        {
            int BasicFunc() => 10;

            MyThreadPool pool = new MyThreadPool(4);
            var task = pool.AddTask(BasicFunc);
            pool.Shutdown();
            Thread.Sleep(500);

            Assert.Equal(10, task.Result);
            Assert.Equal(0, pool.threadsCount());
        }

        [Fact]
        public void ExceptionTest()
        {
            int del()
            {
                int b = 0;
                int a = 5 / b;

                return a;
            }

            MyThreadPool pool = new MyThreadPool(5);
            var task = pool.AddTask(del);
            try
            {
                var result = task.Result;
            }
            catch(Exception e)
            {
                Assert.NotNull(e);
            }
        }
        
    }
}
