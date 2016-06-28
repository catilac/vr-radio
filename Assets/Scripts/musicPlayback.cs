using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class musicPlayback : MonoBehaviour {

	private AudioSource song;
	private List<AudioClip> songs = new List<AudioClip> ();
	private bool isPlaying;
	private string absolutePath = "Assets/OGG";
	private HashSet<int> thumbedDown = new HashSet<int> ();
	private HashSet<int> thumbedUp = new HashSet<int>();
	private FileInfo[] songFiles;
	private List<string> validExtensions = new List<string> {".ogg"};

	private int currentSongIndex = 0;

	private char[] delimiter = { '_' };
	public string currentSongAlbumID;
	public string currentSongTitle;
	public string currentSongArtist;
	public string currentSongAlbum;


	// Use this for initialization
	void Start () {
		isPlaying = false;
		if (song == null) {
			song = gameObject.AddComponent<AudioSource>();
		}
		loadSongs ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp ("p")) { //change input to appropriate controls later
			if (isPlaying) {
				pauseCurrent ();
			} else {
				playCurrent ();
			}
		}
		if (Input.GetKeyUp (KeyCode.RightArrow)) { //change input to appropriate controls later
			skipCurrent ();
		}
		if (Input.GetKeyUp (KeyCode.UpArrow)) {
			thumbsUp ();
		}
		if (Input.GetKeyUp (KeyCode.DownArrow)) {
			thumbsDown ();
		}
	}

	void loadSongs() {
		print ("Loading songs!");
		songs.Clear ();
		DirectoryInfo info = new DirectoryInfo (absolutePath);
		songFiles = info.GetFiles ()
			.Where(f => isValidFileType(f.Name)).ToArray();

		foreach (FileInfo s in songFiles) {
			StartCoroutine (LoadFile (s.FullName));
		}
	}

	bool isValidFileType(string fileName) {
		return validExtensions.Contains (Path.GetExtension (fileName));
	}

	IEnumerator LoadFile(string path) {
		WWW www = new WWW ("file://" + path);
		print ("loading " + path);

		AudioClip clip = www.GetAudioClip (false);
		while (!(clip.loadState == AudioDataLoadState.Loaded)) {
			yield return www;
		}

		print ("finished loading" + path);
		clip.name = Path.GetFileName (path);
		songs.Add (clip);
	}

	void playCurrent() {
		song.clip = songs [currentSongIndex];

		string[] song_info = song.clip.name.Split (delimiter);
		currentSongAlbumID = song_info[0];
		currentSongTitle = song_info[1];
		currentSongArtist = song_info[2];
		currentSongAlbum = song_info[3];

		song.Play ();
		isPlaying = true;

	}

	void pauseCurrent() {
		song.Pause ();
		isPlaying = false;
	}

	void skipCurrent() { //edge case: all songs thumbed down
		incrementSongIndex ();
		song.Stop ();
		while (thumbedDown.Contains(currentSongIndex)) {
			incrementSongIndex ();
		}
		playCurrent ();
	}

	void incrementSongIndex() {
		currentSongIndex = (currentSongIndex + 1) % songs.Count;
	}

	void thumbsDown() {
		thumbedDown.Add (currentSongIndex);
		skipCurrent ();
	}

	void thumbsUp() {
		thumbedUp.Add (currentSongIndex);
	}

}
