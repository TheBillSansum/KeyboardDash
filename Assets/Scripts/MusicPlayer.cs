using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MusicPlayer : MonoBehaviour
{
    #region Public Variables

    public Slider progressSlider;
    public TextMeshProUGUI trackNumberText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI songTitleText;
    public TextMeshProUGUI artistText;
    public Image albumCoverImage;

    public Button playButton;
    public Button stopButton;
    public Button pauseButton;
    public Button nextButton;
    public Button prevButton;

    public SongData[] songs;
    public float volume = 1;

    #endregion

    #region Private Variables

    private int currentTrackIndex = 0;
    public bool isPlaying = false;
    public AudioSource audioSource;

    #endregion

    #region Initialization

    private void Start()
    {
        // Check if music is disabled
        if (PlayerPrefs.GetInt("MusicOff") == 1)
        {
            this.gameObject.SetActive(false);
        }

        // Shuffle songs and play
        ShuffleSongs(songs);
        Play();

        // Add button listeners
        playButton.onClick.AddListener(Play);
        stopButton.onClick.AddListener(Stop);
        pauseButton.onClick.AddListener(Pause);
        nextButton.onClick.AddListener(NextSong);
        prevButton.onClick.AddListener(PreviousSong);

        // Set initial volume and update UI
        UpdateVolume(1); //Set volume to start at 1 (Maximum)
        UpdateUI();
    }

    #endregion

    #region Utility Functions

    /// <summary>
    /// Shuffles the array of SongData objects.
    /// </summary>
    public static void ShuffleSongs(SongData[] songs)
    {
        System.Random rng = new System.Random();

        int n = songs.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            SongData value = songs[k];
            songs[k] = songs[n];
            songs[n] = value;
        }
    }

    /// <summary>
    /// Updates the UI elements with information about the current song.
    /// </summary>
    private void UpdateUI()
    {
        if (songs.Length == 0 || currentTrackIndex < 0 || currentTrackIndex >= songs.Length)
            return;

        SongData currentSong = songs[currentTrackIndex];
        trackNumberText.text = (currentTrackIndex + 1).ToString();
        songTitleText.text = currentSong.title;
        artistText.text = currentSong.artist;

        // Update time text
        int minutes = Mathf.FloorToInt(audioSource.time / 60);
        int seconds = Mathf.FloorToInt(audioSource.time % 60);
        timeText.text = string.Format("{0}:{1:00}", minutes, seconds);

        // Update album cover image
        albumCoverImage.sprite = currentSong.albumCover;
        audioSource.volume = volume;
    }

    #endregion

    #region Playback Control

    /// <summary>
    /// Plays the current song.
    /// </summary>
    public void Play()
    {
        isPlaying = true;
        SongData currentSong = songs[currentTrackIndex];
        audioSource.clip = currentSong.audioClip;
        audioSource.Play();
        Debug.Log("Playing: " + currentSong.title);
    }

    /// <summary>
    /// Stops playback.
    /// </summary>
    public void Stop()
    {
        isPlaying = false;
        audioSource.Stop();
        Debug.Log("Stopped: " + songs[currentTrackIndex].title);
    }

    /// <summary>
    /// Pauses playback.
    /// </summary>
    public void Pause()
    {
        isPlaying = false;
        audioSource.Pause();
        Debug.Log("Paused: " + songs[currentTrackIndex].title);
    }

    /// <summary>
    /// Plays the next song.
    /// </summary>
    public void NextSong()
    {
        Stop();
        currentTrackIndex = (currentTrackIndex + 1) % songs.Length;
        UpdateUI();
        Play();
    }

    /// <summary>
    /// Plays the previous song.
    /// </summary>
    public void PreviousSong()
    {
        Stop();
        currentTrackIndex = (currentTrackIndex - 1 + songs.Length) % songs.Length;
        UpdateUI();
        Play();
    }

    #endregion

    #region Volume Control

    /// <summary>
    /// Updates the volume level.
    /// </summary>
    public void UpdateVolume(float vol)
    {
        volume = vol / 3;
    }

    #endregion

    #region Update

    private void Update()
    {
        // Update progress slider and UI if playing
        if (isPlaying && audioSource.isPlaying)
        {
            progressSlider.value = audioSource.time / audioSource.clip.length;
            UpdateUI();
        }
        // Play next song if finished
        else if (isPlaying && !audioSource.isPlaying)
        {
            NextSong();
        }
    }

    #endregion
}