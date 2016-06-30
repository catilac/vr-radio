using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class musicPlayback : MonoBehaviour {

	private AudioSource song;
	private List<AudioClip> songs = new List<AudioClip> ();
	private bool isPlaying;
	private string absolutePath = "Assets/";
	private HashSet<int> thumbedDown = new HashSet<int> ();
	private HashSet<int> thumbedUp = new HashSet<int>();
	private FileInfo[] songFiles;
	private List<string> validExtensions = new List<string> {".ogg"};

	private int currentSongIndex = 0;
	private int numSongs;

	private char[] delimiters = { '_', '.' };
	public string currentSongAlbumID;
	public string currentSongTitle;
	public string currentSongArtist;
	public string currentSongAlbum;

	public Texture2D currentAlbumArt = new Texture2D(2, 2);

	// Use this for initialization
	void Start () {
		isPlaying = false;
		if (song == null) {
			song = gameObject.AddComponent<AudioSource>();
		}
		loadAlbumArt ("default.jpg");
		loadSongs ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!song.isPlaying && isPlaying) {
			skipCurrent ();
		}
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
		if (Input.GetKeyUp (KeyCode.UpArrow)) { //change input to appropriate controls later
			thumbsUp ();
		}
		if (Input.GetKeyUp (KeyCode.DownArrow)) { //change input to appropriate controls later
			thumbsDown ();
		}
	}

	void loadSongs() {
		songs.Clear ();
		DirectoryInfo info = new DirectoryInfo (absolutePath + "OGG");
		songFiles = info.GetFiles ()
			.Where(f => isValidFileType(f.Name)).ToArray();
		numSongs = songFiles.Length;

		foreach (FileInfo s in songFiles) {
			StartCoroutine (LoadFile (s.FullName));
		}
	}

	bool isValidFileType(string fileName) {
		return validExtensions.Contains (Path.GetExtension (fileName));
	}

	IEnumerator LoadFile(string path) {
		WWW www = new WWW ("file://" + path);

		AudioClip clip = www.GetAudioClip (false);
		while (!(clip.loadState == AudioDataLoadState.Loaded)) {
			yield return www;
		}

		clip.name = Path.GetFileName (path);
		songs.Add (clip);
		print ("Finished loading " + path);

	}

	void loadAlbumArt(string albumID) {
		byte[] albumArt = File.ReadAllBytes (absolutePath + "Album Art/" + albumID);
		currentAlbumArt.LoadImage (albumArt);
		gameObject.GetComponent<Renderer> ().material.mainTexture = currentAlbumArt;
	}


	//Music playback controls
	public void playCurrent() {
		song.clip = songs [currentSongIndex];

		string[] song_info = song.clip.name.Split (delimiters);
		if (currentSongAlbumID != song_info[0]) {
			currentSongAlbumID = song_info[0];
			currentSongTitle = song_info[1];
			currentSongArtist = song_info[2];
			currentSongAlbum = song_info[3];

			loadAlbumArt (currentSongAlbumID + ".jpg");
		}

		song.Play ();
		isPlaying = true;
	}

	public void pauseCurrent() {
		song.Pause ();
		isPlaying = false;
	}

	public void skipCurrent() { //edge case: all songs thumbed down
		incrementSongIndex ();
		song.Stop ();
		if (thumbedDown.Count == numSongs) { //all songs thumbed down
			loadAlbumArt("no_cover.png");
			return;
		}
		while (thumbedDown.Contains(currentSongIndex)) {
			incrementSongIndex ();
		}
		playCurrent ();
	}

	void incrementSongIndex() {
		currentSongIndex = (currentSongIndex + 1) % songs.Count;
	}

	public void thumbsDown() {
		thumbedDown.Add (currentSongIndex);
		skipCurrent ();
	}

	public void thumbsUp() {
		thumbedUp.Add (currentSongIndex);
	}

	void OnTriggerEnter(Collider Other){
		HandAnimator hand = Other.gameObject.GetComponent<HandAnimator> ();
		if (hand != null) {
			if (hand.thumbsDown ()) {
				this.thumbsDown ();
			} else if (hand.thumbsUp ()) {
				this.thumbsUp ();
			}
		}
	}

}
