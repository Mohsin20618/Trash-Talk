using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoggingScript : MonoBehaviour
{
	struct Log
	{
		public string message;
		public string stackTrace;
		public LogType type;
	}

	/// <summary>
	/// The hotkey to show and hide the console window.
	/// </summary>
	public KeyCode toggleKey = KeyCode.BackQuote;

	List<Log> logs = new List<Log>();
	Vector2 scrollPosition;
	bool show;
	bool collapse;

	// Visual elements:

	static readonly Dictionary<LogType, Color> logTypeColors = new Dictionary<LogType, Color>() {
		{ LogType.Assert, Color.white },
		{ LogType.Error, Color.red },
		{ LogType.Exception, Color.red },
		{ LogType.Log, Color.white },
		{ LogType.Warning, Color.yellow },
	};

	const int margin = 20;

	Rect windowRect = new Rect(margin, margin, Screen.width - (margin), Screen.height - (margin));
	Rect titleBarRect = new Rect(0, 0, 10000, 20);
	GUIContent clearLabel = new GUIContent("Clear", "Clear the contents of the console.");
	GUIContent collapseLabel = new GUIContent("Collapse", "Hide repeated messages.");

	private void Awake()
	{
		//if (GameSparksManager.instance.gameConfig.debug)
		//{
		//	this.enabled = true;
		//}
		//else
		//{
		//	this.enabled = false;
		//}
	}

	void OnEnable()
	{
		Application.logMessageReceived += HandleLog;// ; (HandleLog);
	}

	void OnDisable()
	{
		Application.logMessageReceived -= HandleLog;
	}

	void Update()
	{
		if (Input.GetKeyDown(toggleKey))
		{
			show = !show;
		}
	}

	void OnGUI()
	{
		if (GUI.Button(new Rect(Screen.width - 140, Screen.height - 40, 120, 30), "Show Console"))
		{
			show = !show;


		}
		if (!show)
		{
			return;
		}

		windowRect = GUILayout.Window(123456, windowRect, ConsoleWindow, "Console");
	}

	/// <summary>
	/// A window that displayss the recorded logs.
	/// </summary>
	/// <param name="windowID">Window ID.</param>
	void ConsoleWindow(int windowID)
	{
		scrollPosition = GUILayout.BeginScrollView(scrollPosition);

		// Iterate through the recorded logs.
		for (int i = 0; i < logs.Count; i++)
		{
			var log = logs[i];

			// Combine identical messages if collapse option is chosen.
			if (collapse)
			{
				var messageSameAsPrevious = i > 0 && log.message == logs[i - 1].message;

				if (messageSameAsPrevious)
				{
					continue;
				}
			}

			GUI.contentColor = logTypeColors[log.type];
			GUILayout.Label(log.message);
		}

		GUILayout.EndScrollView();

		GUI.contentColor = Color.white;

		GUILayout.BeginHorizontal();

		if (GUILayout.Button(clearLabel))
		{
			logs.Clear();
		}

		collapse = GUILayout.Toggle(collapse, collapseLabel, GUILayout.ExpandWidth(false));

		GUILayout.EndHorizontal();

		// Allow the window to be dragged by its title bar.
		GUI.DragWindow(titleBarRect);
	}

	/// <summary>
	/// Records a log from the log callback.
	/// </summary>
	/// <param name="message">Message.</param>
	/// <param name="stackTrace">Trace of where the message came from.</param>
	/// <param name="type">Type of message (error, exception, warning, assert).</param>
	void HandleLog(string condition, string stackTrace, LogType type)
	{
		logs.Add(new Log()
		{
			message = condition,
			stackTrace = stackTrace,
			type = type,
		});
	}

}
