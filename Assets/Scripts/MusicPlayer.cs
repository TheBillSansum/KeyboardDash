using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MusicPlayer : MonoBehaviour
{
    public Slider progressSlider;
    public TextMeshProUGUI trackNumberText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI songTitleText;
    public TextMeshProUGUI artistText;
    public Image albumCoverImage; // Add this field in the Inspector and assign the Image component

    public Button playButton;
    public Button stopButton;
    public Button pauseButton;
    public Button nextButton;
    public Button prevButton;

    public SongData[] songs;
    private int currentTrackIndex = 0;
    public float volume = 1;
    private bool isPlaying = false;

    public AudioSource audioSource;

    private void Start()
    {
        Play();

        playButton.onClick.AddListener(Play);
        stopButton.onClick.AddListener(Stop);
        pauseButton.onClick.AddListener(Pause);
        nextButton.onClick.AddListener(NextSong);
        prevButton.onClick.AddListener(PreviousSong);

        UpdateUI();
    }

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
        albumCoverImage.sprite = currentSong.albumCover; // Assuming albumCover is a Sprite field in SongData
        audioSource.volume = volume;
    }

    public void Play()
    {
        isPlaying = true;
        SongData currentSong = songs[currentTrackIndex];
        audioSource.clip = currentSong.audioClip;
        audioSource.Play();
        Debug.Log("Playing: " + currentSong.title);
    }

    public void Stop()
    {
        isPlaying = false;
        audioSource.Stop();
        Debug.Log("Stopped: " + songs[currentTrackIndex].title);
    }

    public void Pause()
    {
        isPlaying = false;
        audioSource.Pause();
        Debug.Log("Paused: " + songs[currentTrackIndex].title);
    }

    public void NextSong()
    {
        Stop();
        currentTrackIndex = (currentTrackIndex + 1) % songs.Length;
        UpdateUI();
        Play();
    }

    public void PreviousSong()
    {
        Stop();
        currentTrackIndex = (currentTrackIndex - 1 + songs.Length) % songs.Length;
        UpdateUI();
        Play();
    }

    public void UpdateVolume(float vol)
    {
        volume = vol / 3 ;
    }

    private void Update()
    {
        if (isPlaying && audioSource.isPlaying)
        {
            progressSlider.value = audioSource.time / audioSource.clip.length;
            UpdateUI();
        }
        else if (isPlaying && !audioSource.isPlaying)
        {
            NextSong();
        }
    }
}