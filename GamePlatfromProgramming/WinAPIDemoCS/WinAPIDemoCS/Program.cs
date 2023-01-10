﻿using System;
using System.Threading;
using WinAPIDemoCS;
//WinAPI의 콜백과 메세지구조를 흉내내어 콘솔프로그래밍에서 스레드와 프로세스의 구조를 이해하기
//1. 콜백,메세지,스레드,프로스세스,큐
//2. 프로그램은 종료시까지 반복되야하므로, 루프를 이용하여 종료 입력 전까지 프로그램이 작동되어야한다.
//3. 메세지를 입력하여 적절한 명령을 수행해야하므로, enum 을 정의하고, 값에 따라  저장하는 enum의 값을 다르게 처리를 다르게 만든다.
//4. 입력을 대하는 대기시켯을때 메세지가 처리되지않고 대기된다.
namespace WinAPIDemoCS
{
    class WinAPI
    {
        public enum WM_MSG { CREATE, COMMOND, PAINT, DESTROY, MAX };

        WM_MSG m_eMsg;
        bool m_isLoop = true;

        public WM_MSG MSG { set => m_eMsg = value; get => m_eMsg; } //인덱서: 프라이빗멤버에 접근하는 세터와 게터를 간략하게 코드 작성이 가능함.
        public bool Loop { get => m_isLoop; } 

        public WinAPI(WM_MSG msg)
        {
            m_eMsg = msg;
        }

        public void WndProc()
        {
            while (m_isLoop)
            {
                switch (m_eMsg)
                {
                    case WM_MSG.CREATE:
                        Console.WriteLine(m_eMsg + ": 초기화");
                        m_eMsg = WM_MSG.COMMOND;
                        break;
                    case WM_MSG.COMMOND:
                        Console.WriteLine(m_eMsg + ": 명령을 입력하세요!");
                        for (WM_MSG i = WM_MSG.CREATE; i < WM_MSG.MAX; i++)
                            Console.Write("{0}:{1} ", (int)i, i.ToString());
                        Console.WriteLine();
                        break;
                    case WM_MSG.PAINT:
                        Console.WriteLine(m_eMsg + ": 화면을 그립니다.");
                        break;
                    case WM_MSG.DESTROY:
                        Console.WriteLine(m_eMsg + ": 프로그램을 종료합니다.");
                        m_isLoop = false;
                        break;
                    default:
                        break;
                };
                Thread.Sleep(2000);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)//프로세스: 프로그램내에서 기본적인 흐름.
        {
            WinAPI cWinAPI = new WinAPI(WinAPI.WM_MSG.CREATE);
            ThreadStart threadStart = new ThreadStart(cWinAPI.WndProc);
            Thread cThread = new Thread(threadStart);
            cThread.Start();

            while (cWinAPI.Loop)
            {
                string strMsg = Console.ReadLine();
                WinAPI.WM_MSG eMSG = (WinAPI.WM_MSG)int.Parse(strMsg);
                if(eMSG != WinAPI.WM_MSG.DESTROY)
                    cWinAPI.MSG = eMSG;
            }
        }
    }
}