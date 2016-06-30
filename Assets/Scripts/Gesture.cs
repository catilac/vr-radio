using System;
using UnityEngine;
using System.Collections;

public class Gesture
{
	static GameObject gestureDrawing;
	GestureTemplates m_Templates;

	public musicPlayback player;

	ArrayList pointArr;
	static int mouseDown;

	public Gesture ()
	{
		m_Templates = new GestureTemplates ();
		pointArr = new ArrayList ();

		gestureDrawing = GameObject.Find ("gesture");
	}

	public void StartGestureRecognition(Vector2 pos)
	{
		pointArr.Add (pos);
	}

	public void StopGestureRecognition(){

		if (Input.GetKey (KeyCode.LeftControl))
		{
			// if CTRL is held down, the script will record a gesture. 
			mouseDown = 0;
			GestureRecognizer.recordTemplate(pointArr);

		}
		else
		{
			mouseDown = 0;

			// start recognizing! 
			GestureRecognizer.startRecognizer(pointArr, player);

			pointArr.Clear();

		}

	}

	void OnGUI ()
	{
		if(GestureRecognizer.recordDone == 1)
		{ 
			GUI.Window (0, new Rect (350, 220, 300, 100), DoMyWindow, "Save the template?");
		}
	}

	void DoMyWindow (int windowID)
	{
		GestureRecognizer.stringToEdit = GUILayout.TextField(GestureRecognizer.stringToEdit);

		if (GUI.Button (new Rect (100,50,50,20), "Save"))
		{
			ArrayList temp = new ArrayList();
			ArrayList a = (ArrayList)GestureTemplates.Templates[GestureTemplates.Templates.Count - 1];

			for (int i = 0; i < GestureRecognizer.newTemplateArr.Count; i++)
				temp.Add(GestureRecognizer.newTemplateArr[i]);

			GestureTemplates.Templates.Add(temp);
			GestureTemplates.TemplateNames.Add(GestureRecognizer.stringToEdit);
			GestureRecognizer.recordDone = 0;
			GestureRecognizer.newTemplateArr.Clear();
		}

		if (GUI.Button (new Rect (160,50,50,20), "Cancel")) 
		{
			GestureRecognizer.recordDone = 0;
		}
	}
}

