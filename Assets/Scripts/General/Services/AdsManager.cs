using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using CandyCoded.env;

public class AdsManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener {
    
    public static AdsManager Instance;

    private string _gameId = "";
    private string _adUnitId = "Interstitial_Android";
    private GameStateManager _gameStateManager;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        _gameStateManager = FindObjectOfType<GameStateManager>();
        
        env.TryParseEnvironmentVariable("GAME_ID_ANDROID", out _gameId);
        
        Advertisement.Initialize(_gameId, false); 
        LoadAd();
    }

    public void LoadAd() {
        Debug.Log("Cargando anuncio intersticial...");
        Advertisement.Load(_adUnitId, this); 
    }

    public void PlayAd() {
        Debug.Log("Mostrando anuncio...");
        Advertisement.Show(_adUnitId, this);
    }
    
    public void OnUnityAdsAdLoaded(string placementId) {
        Debug.Log($"Anuncio cargado exitosamente: {placementId}");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) {
        Debug.LogError($"Error cargando anuncio {placementId}: {error.ToString()} - {message}");
    }
    
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) {
        Debug.LogError($"Error mostrando anuncio {placementId}: {error.ToString()} - {message}");
        if (_gameStateManager != null) {
            _gameStateManager.Continue();
        }
    }

    public void OnUnityAdsShowStart(string placementId) {
        Debug.Log($"Anuncio iniciado: {placementId}");
    }

    public void OnUnityAdsShowClick(string placementId) {
        Debug.Log($"Clic en anuncio: {placementId}");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState) {
        Debug.Log($"Anuncio completado: {placementId}. Estado: {showCompletionState}");
        
        if (_gameStateManager != null) {
            _gameStateManager.Continue();
        }

        LoadAd(); 
    }
}