using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject chip;
    public GameObject player;
    public Player playerComponent;
    public SfxPlayer sfxPlayer;
    public Transform aim;
    public Transform cameraFollow;

    private AudioSource _audioSource;
    private Camera _mainCam;
    private Box[] _boxes;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        _audioSource = GetComponent<AudioSource>();
        _mainCam = Camera.main;
        _boxes = FindObjectsOfType<Box>();
        playerComponent = player.GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
        
        aim.position = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        cameraFollow.position = aim.position * 0.1f + player.transform.position * 0.9f;
        AssignPlayerBox();
    }

    private void AssignPlayerBox()
    {
        bool hasBox = false;
        foreach (var box in _boxes)
        {
            if (Vector3.Distance(box.transform.position, player.transform.position) <= 1)
            {
                playerComponent.box = box;
                hasBox = true;
                break;
            }
        }

        if (!hasBox)
        {
            playerComponent.box = null;
            foreach (var box in _boxes)
            {
                box.GetComponent<Instruction>().ToggleInstruction(false, "E");
            }
        }
    }

    public bool InInsertedBoxRange(Vector3 pos)
    {
        bool result = false;
        foreach (var box in _boxes)
        {
            if (box.inserted && Vector3.Distance(box.transform.position, pos) <= .48f)
            {
                result = true;
                break;
            }
        }

        return result;
    }

    public void TryReversePlay(bool resverse)
    {
        if (resverse)
        {
            if (!player.GetComponent<Player>().Equipped)
                _audioSource.pitch = -1;
        }
        else
        {
            _audioSource.pitch = 1;
        }
    }
}
