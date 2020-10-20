#region License
/*
 * TestSocketIO.cs
 *
 * The MIT License
 *
 * Copyright (c) 2014 Fabio Panettieri
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#endregion

using System.Collections;
using UnityEngine;
using SocketIO;
using System.Collections.Generic;

public class TestSocketIO : MonoBehaviour
{
	private SocketIOComponent socket;
	Coroutine Coroutine_BeepBoop1, Coroutine_BeepBoop2;

	public void Start() 
	{
		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();

		socket.On("open", TestOpen);
		socket.On("boop", TestBoop);
		socket.On("error", TestError);
		socket.On("close", TestClose);

		//Coroutine_BeepBoop1 = StartCoroutine(BeepBoop());
		//Coroutine_BeepBoop2 = StartCoroutine(BeepBoop2());
		//StartCoroutine(StopBeepBoop());
	}

	private IEnumerator BeepBoop()
	{
		Dictionary<string, string> data = new Dictionary<string, string>();
		//data.Add("msg", "Hello Server");
		JSONObject jsonObject;

		for (int i = 0; i < 10; i++)
		{
			yield return new WaitForSeconds(1f);
			data["msg"] = $"Hello Server {i}";
			jsonObject = new JSONObject(data);
			socket.Emit("beep", jsonObject);
		}
	}

	private IEnumerator BeepBoop2()
	{
		Dictionary<string, string> data = new Dictionary<string, string>();
		//data.Add("msg", "Hello Server");
		JSONObject jsonObject;

		for (int i = 0; i < 10; i++)
		{
			yield return new WaitForSeconds(1f);
			data["msg"] = $"안녕 서버!!! {i}";
			jsonObject = new JSONObject(data);
			socket.Emit("beep", jsonObject);
		}
	}

	private IEnumerator StopBeepBoop()
	{
        //yield return new WaitForSeconds(4f);
        //StopCoroutine(Coroutine_BeepBoop1);
        //yield return new WaitForSeconds(4f);
        //StopCoroutine(Coroutine_BeepBoop2);

        yield return new WaitForSeconds(5f);
        StopAllCoroutines();
	}

	public void TestOpen(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
	}

	public void TestBoop(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Boop received: " + e.name + " " + e.data);

		if (e.data == null) { return; }

		Debug.Log("MSG: " + e.data.GetField("msg").str);
	}

	public void TestError(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
	}
	
	public void TestClose(SocketIOEvent e)
	{	
		Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
	}
}
