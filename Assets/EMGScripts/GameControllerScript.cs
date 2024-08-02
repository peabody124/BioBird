using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;

public class GameControllerScript : MonoBehaviour
{
    // private const int SPLIT_SCREEN_WIDTH_INTO_EQUAL_SEGMENTS_COUNT = 32; // sets number of points to generate for the curve
    // private const int SCREEN_SEGMENT_FOR_SPACESHIP_X_POSITION_INDEX = 5;

    [Serializable]
    public class Event : UnityEvent { };
    // public Event setCoinTypeNormal;
    // public Event setCoinTypeBonus;
    // public Event setStartGameActive;
    public Event updateAndroidTrialLog;
    // public Event updateEMGdataAtCurvePeakCrossing;
    // public Event UpdateEmgMaxFiltered;

    [Serializable]
    public class FloatEvent : UnityEvent<float> { };
    // public FloatEvent sendCommandSignalToSpaceship;
    // public FloatEvent sendMountainBaseHeight;
    // public FloatEvent spaceshipPositionXCallback;
    // public FloatEvent setSpaceshipAltitudeAtInitialization;
    // used to update the game log
    // public FloatEvent updateCoinAltitudeScaledLog;
    // public FloatEvent updateCoinHeightScaledLog;
    // public FloatEvent updateCurveAmplitudeScaledLog;
    // public FloatEvent updateCurveSpeedScaledLog;
    // public FloatEvent updateCurveWidthScaledLog;
    // public FloatEvent updateSpaceshipCommandSignalAtCurvePeakScaledLog;
    // public FloatEvent updateSpaceshipCommandSignalAtCrashLog;

    // public FloatEvent updateCurveAmplitudeScaledUI;
    // public FloatEvent updateCurveSpeedScaledLogUI;
    // public FloatEvent updateCurveWidthScaledLogUI;
    // public FloatEvent updateCurveSuperMaxProbabilityUI;
    // public FloatEvent updateSpaceshipScaledAltitudeUI;
    // public FloatEvent updateCointAltitudeUI;

    // [Serializable]
    // public class IntEvent : UnityEvent<int> { };
    // public IntEvent updatePlayerLifeUI;
    // public IntEvent updateNormalPlayerScoreUI;
    // public IntEvent updateBonusPlayerScoreUI;
    // public IntEvent updateTrialInfoWithTrialNumber;

    // [Serializable]
    // public class BoolEvent : UnityEvent<bool> { };
    // public BoolEvent updateTrialPlayerCrashRecord;

    // [Serializable]
    // public class UpdateCoinPosition : UnityEvent<Vector3> { };
    // public UpdateCoinPosition coinPositionCallback;
    // public static Vector3 curvePeakPoint;
    // public static float spaceshipCommandPositionX;

    // [Serializable]
    // public class UpdateCurveSpaceshipCollisionPosition : UnityEvent<Vector3, Vector3, Vector3> { };
    // public UpdateCurveSpaceshipCollisionPosition curveSpaceshipCollisionCallback;

    // [Serializable]
    // public class SendCurvePointsToRenderer : UnityEvent<Vector3[]> { };
    // public SendCurvePointsToRenderer sendCurvePointsToRenderer;

    private float EMGSignal;

    // private float safeScreenBottomCoordinate;
    // private float safeScreenTopCoordinate;

    // private float spaceshipHeight;
    // private Vector2 spaceshipCommandPositionScaledByScreenSize;
    // private float maxSpaceshipAltitude; // goes off the screen
    // private float minSpaceshipAltitude;

    // private float coinHeight;
    // private float coinPaddedHeight;

    // Curve Parameters
    // paramters used to calculate the position of the curve on the screen
    // private float muMin;
    // private float muMax;
    // private float muStep;
    // private float lastMuStep;

    // private float a; // height of the curve's peak
    // private float sigma; // sets the width of the curve (how fat it is)
    // private float mu; // specifies where on the screen curve's peak is located: 2: right, -1: left 

    // private Vector3[] curveGeneratedPoints;      // number of points the curve should contain
    // private float supermaxProbability = 0.2f;  // controls the max amplitude generation frequency
    // private float maxCurveAmplitude; // used for curve parameter generation, depends on screen resolution
    // private float minCurveAmplitude; 
    // private float targetScreenHeight;
    // private float targetScreenWidth;

    // Camera cam;

    // enum GameState { INITIALIZE_GAME, CALIBRATE_ADAPTIVE_CONTROL, WAIT_FOR_USER, TRIAL, UPDATEINFO };
    // GameState gameState = GameState.INITIALIZE_GAME;
    //GameState gameState = GameState.TRIAL;

    // private int playerLife = 100;
    // private int playerNormalScore = 0;      // gold coin     
    // private int playerBonusScore = 0; // red coin 
    // private float spaceshipCommandSignalScaledByMinMaxAltitudeWithHeightOffset; 

    // private int trialNumber = 0;
    // private bool didPlayerCrashThisTrial = false;
    // private float spaceshipControlSignalAtCrash;

    // private float coinFinalAltitude;
    // private float coinAltitudeScaledByScreenSize;
    // private float curveAmplitude;
    // private float curveAmplitudeScaledByScreenSize;
    // private float coinHeightScaledByScreenSize;

    // private bool didPlayerCrossCurvePeakPointThisTrial = false;

    // private void Awake()
    // {
    //     Input.backButtonLeavesApp = true; // allows you to exit the App using Android back button
    //     SetScreenParameters();
    // }

    // private void Start()
    // {
    //     updatePlayerLifeUI.Invoke(playerLife);
    //     updateNormalPlayerScoreUI.Invoke(playerNormalScore);
    //     updateBonusPlayerScoreUI.Invoke(playerBonusScore);
    //     setSpaceshipAltitudeAtInitialization.Invoke(minSpaceshipAltitude);
    // }

//     void FixedUpdate()
//     {
//         switch (gameState)
//         {
//             case GameState.INITIALIZE_GAME:
//                 sendMountainBaseHeight.Invoke(safeScreenBottomCoordinate);

//                 // curve generation setting fields
//                 mu = -1.1f;        // specifies where on the screen the bump is located: 2: right, -1: left 
//                 sigma = 0.15f;      // specifies the width of the bump
//                 muMin = 1f; //0.5f
//                 muMax = 1.5f; //1.5f
//                 muStep = 0.02f;

//                 curveGeneratedPoints = new Vector3[SPLIT_SCREEN_WIDTH_INTO_EQUAL_SEGMENTS_COUNT + 1];
//                 GenerateCurveParameters();
//                 GenerateGaussianCurve(SPLIT_SCREEN_WIDTH_INTO_EQUAL_SEGMENTS_COUNT, a, sigma, mu);
//                 sendCurvePointsToRenderer.Invoke(curveGeneratedPoints);
//                 GenerateSpaceshipPositionX();
//                 updateCoinHeightScaledLog.Invoke(ScaleCoinPaddedHeightByScreenSize());
//                 SetGameStateToWaitForUser();
//                 break;

//             case GameState.CALIBRATE_ADAPTIVE_CONTROL:
// /*                stateString = "WAITING";
//                 SendCommandSignal(controlSignal);
//                 TurnOnStartGameButton.Invoke(0f);
//                 StartRocketEngine.Invoke(0f);
//                 SetEmissionRateEvent.Invoke(ScaleInput(controlSignal, emissionsLowerBoundValue, emissionsUpperBoundValue, 5f, 45f));*/
//                 break;

//             case GameState.WAIT_FOR_USER:
//                 //spaceshipCommandSignalScaledByMinMaxAltitudeWithHeightOffset = CalculateSpaceshipCommandSignal(EMGSignal);
//                 spaceshipCommandSignalScaledByMinMaxAltitudeWithHeightOffset = CalculateSpaceshipCommandSignal(EMGSignal);
//                 sendCommandSignalToSpaceship.Invoke(spaceshipCommandSignalScaledByMinMaxAltitudeWithHeightOffset);
//                 updateSpaceshipScaledAltitudeUI.Invoke(ScaleSpaceshipAltitudeByScreenSize());
//                 break;

//             case GameState.TRIAL:
//                 // curve generator 
//                 GenerateCurveParameters();
//                 GenerateGaussianCurve(SPLIT_SCREEN_WIDTH_INTO_EQUAL_SEGMENTS_COUNT, a, sigma, mu);
//                 sendCurvePointsToRenderer.Invoke(curveGeneratedPoints);
//                 // spaceship command

//                 spaceshipCommandSignalScaledByMinMaxAltitudeWithHeightOffset = CalculateSpaceshipCommandSignal(EMGSignal);
//                 sendCommandSignalToSpaceship.Invoke(spaceshipCommandSignalScaledByMinMaxAltitudeWithHeightOffset);
//                 updateSpaceshipScaledAltitudeUI.Invoke(ScaleSpaceshipAltitudeByScreenSize());
//                 updateCurveAmplitudeScaledUI.Invoke(ScaleCurveAmplitudeByScreenSize());
//                 updateCurveSpeedScaledLogUI.Invoke(muStep);
//                 updateCurveWidthScaledLogUI.Invoke(sigma);
//                 updateCurveSuperMaxProbabilityUI.Invoke(supermaxProbability);
//                 updateCointAltitudeUI.Invoke(ScaleCoinAltitudeByScreenSize());

//                 // when the curve peak point crosses the x position of the spaceship, do the following:
//                 // (1) update log with curve amplitude
//                 // (2) update log with curve width
//                 // (3) update log with curve speed
//                 // (4) update log with coin altitude
//                 // (5) update log with the spaceship altitude 
//                 if (curvePeakPoint.x <= spaceshipCommandPositionX)
//                 {
//                     if (didPlayerCrossCurvePeakPointThisTrial)
//                     {
//                         didPlayerCrossCurvePeakPointThisTrial = false;
//                         updateCoinAltitudeScaledLog.Invoke(ScaleCoinAltitudeByScreenSize());
//                         updateSpaceshipCommandSignalAtCurvePeakScaledLog.Invoke(ScaleSpaceshipAltitudeByScreenSize());
//                         updateCurveAmplitudeScaledLog.Invoke(ScaleCurveAmplitudeByScreenSize());
//                         updateCurveWidthScaledLog.Invoke(sigma);
//                         updateCurveSpeedScaledLog.Invoke(muStep);
//                         updateEMGdataAtCurvePeakCrossing.Invoke();
//                     }
//                 }
//                 break;
//         }
//     }

    public void EMGSignalObserver(float scaledEMGSignal) 
    {
        EMGSignal = scaledEMGSignal;
    }

    public float ScaleInput(float x, float x_min, float x_max, float a, float b)
    {
        float result = a + ((x - x_min) * (b - a)) / (x_max - x_min);
        return result;
    }

    // private void SetScreenParameters()
    // {
    //     cam = Camera.main;
    //     targetScreenHeight = Screen.height;
    //     targetScreenWidth = Screen.width;
    //     safeScreenBottomCoordinate = Screen.height * 0.1f;
    //     safeScreenTopCoordinate = Screen.height * 0.9f;
    // }

    // private void CalculateCurveMinMaxAmplitude()
    // {
    //     minCurveAmplitude = safeScreenBottomCoordinate;
    //     //maxCurveAmplitude = safeScreenTopCoordinate - coinPaddedHeight * 3;
    //     maxCurveAmplitude = safeScreenTopCoordinate - coinPaddedHeight;
    // }

    // public void SpaceshipHeightObserver(float height)
    // {
    //     spaceshipHeight = height;
    //     SetSpaceshipMaxMinAltitude();
    // }

    // private Vector2 CalculateGameObjectScaledScreenPosition(float commandX, float commandY, float objectHeight, float safeScreenBot, float safeScreenTop)
    // {
    //     float minY = safeScreenBot;// + (objectHeight / 2);
    //     float maxY = safeScreenTop;// + (objectHeight / 2);
    //     return new Vector2(commandX / Screen.width, (commandY - minY) / (maxY - minY));
    // }

    // private void SetSpaceshipMaxMinAltitude()
    // {
    //     float offset = Screen.height * 0.01f; // offset to avoid collision
    //     maxSpaceshipAltitude = Screen.height + (spaceshipHeight / 2);
    //     minSpaceshipAltitude = safeScreenBottomCoordinate + (spaceshipHeight / 2) + offset;
    // }

    // public void CoinPaddedSizeObserver(float width, float height)
    // {
    //     coinHeight = height;
    //     coinPaddedHeight = height;
    //     CalculateCurveMinMaxAmplitude();
    // }


    // #region Curve 
    // void GenerateGaussianCurve(int pointCount, float a, float sigma, float mu)
    // {
    //     List<float> yCurvePoints = new List<float>();
    //     float width = targetScreenWidth / (pointCount);
    //     for (int i = 0; i < pointCount + 1; i++)
    //     {
    //         curveGeneratedPoints[i] = new Vector3(
    //             (i * width),
    //             (a * Mathf.Exp((-Mathf.Pow(ScaleInput(i * width, 0, targetScreenWidth, 0, 1) - mu, 2) / 2 / Mathf.Pow(sigma, 2))) + minCurveAmplitude),
    //             0f
    //             );
    //         yCurvePoints.Add(curveGeneratedPoints[i].y);
    //     }
    //     var maxIndex = yCurvePoints.IndexOf(yCurvePoints.Max());
    //     curvePeakPoint = new Vector3(curveGeneratedPoints[maxIndex].x, curveGeneratedPoints[maxIndex].y, 1f);
    //     coinPositionCallback.Invoke(curvePeakPoint);
    //     GenerateSpaceshipCurveCollisionPoints(curveGeneratedPoints);
    // }

    // /// <summary>
    // /// Generates the parameters used to create a curve that resembles a Guassian Distribution.
    // /// These are global parameters. 
    // /// (1) mu      - specifies the location of the curve on the given target screen.
    // /// (2) sigma   - width of the curve.
    // /// (3) a       - amplitude of the curve. 
    // /// </summary>
    // void GenerateCurveParameters()
    // {
    //     if (mu > -muMin)
    //     {
    //         mu -= muStep;
    //     }
    //     else
    //     {
    //         // update values for the next trial
    //         mu = muMax;
    //         if (UnityEngine.Random.Range(0.0f, 1.0f) <= supermaxProbability) // generate large high curve 
    //         {
    //             a = maxCurveAmplitude;
    //             setCoinTypeBonus.Invoke();
    //         }
    //         else // generate curve of ordinary amplitude
    //         {
    //             a = maxCurveAmplitude * UnityEngine.Random.Range(0.2f, 1.0f);
    //             setCoinTypeNormal.Invoke();
    //         }
    //         sigma = UnityEngine.Random.Range(0.15f, 0.3f);
    //         UpdateEmgMaxFiltered.Invoke();
    //         updateTrialPlayerCrashRecord.Invoke(didPlayerCrashThisTrial);
    //         if (didPlayerCrashThisTrial)
    //         {
    //             updateSpaceshipCommandSignalAtCrashLog.Invoke(ScaleSpaceshipAltitudeAtCrashByScreenSize());
    //         }
    //         else
    //         {
    //             updateSpaceshipCommandSignalAtCrashLog.Invoke(0.0f);
    //         }
    //         if(gameState == GameState.TRIAL)
    //         {
    //             updateAndroidTrialLog.Invoke();
    //             trialNumber++;
    //             didPlayerCrashThisTrial = false;
    //             updateTrialInfoWithTrialNumber.Invoke(trialNumber);
    //             curveAmplitude = a;
    //             didPlayerCrossCurvePeakPointThisTrial = true;
    //         }
    //     }
    // }

    // private void GenerateSpaceshipPositionX()
    // {
    //     //Debug.Log("Called GenerateSpaceshipPositionX --> command = " + spaceshipCommandPositionX );
    //     spaceshipCommandPositionX = curveGeneratedPoints[SCREEN_SEGMENT_FOR_SPACESHIP_X_POSITION_INDEX].x;
    //     spaceshipPositionXCallback.Invoke(spaceshipCommandPositionX);

    // }

    // private void GenerateSpaceshipCurveCollisionPoints(Vector3[] curvePoints)
    // {
    //     Vector3 curvePointAtSpaceshipX = curvePoints[SCREEN_SEGMENT_FOR_SPACESHIP_X_POSITION_INDEX];
    //     Vector3 leftCurvePoint = curvePoints[SCREEN_SEGMENT_FOR_SPACESHIP_X_POSITION_INDEX - 1];
    //     Vector3 rightCurvePoint = curvePoints[SCREEN_SEGMENT_FOR_SPACESHIP_X_POSITION_INDEX + 1];
    //     curveSpaceshipCollisionCallback.Invoke(curvePointAtSpaceshipX, leftCurvePoint, rightCurvePoint);
    // }
    // #endregion

    // public void SetGameStateToTrial()
    // {
    //     gameState = GameState.TRIAL;
    // }

    // public void StartGame()
    // {
    //     gameState = GameState.TRIAL;
    //     //ChangePlayerState(PlayerState.FLYING);
    //     //isPlaying = true;
    // }

    // public void SetGameStateToWaitForUser()
    // {
    //     gameState = GameState.WAIT_FOR_USER;
    //     setStartGameActive.Invoke();
    // }

    // public void SetGameStateToInitialize()
    // {
    //     gameState = GameState.INITIALIZE_GAME;
    // }

    // public void IncreaseNormalScore()
    // {
    //     playerNormalScore += 1;
    //     updateNormalPlayerScoreUI.Invoke(playerNormalScore);
    // }

    // public void IncreaseBonusScore()
    // {
    //     playerBonusScore += 1;
    //     updateBonusPlayerScoreUI.Invoke(playerBonusScore);
    // }
    // public void DecreaseLife()
    // {
    //     playerLife -= 1;
    //     updatePlayerLifeUI.Invoke(playerLife);
    // }

    // public void SetPlayerCrashStatusToCrashed(float crashAltitude)
    // {
    //     spaceshipControlSignalAtCrash = crashAltitude;
    //     didPlayerCrashThisTrial = true;
    // }

    // private float CalculateSpaceshipCommandSignal(float EMGSignal)
    // {
    //     float commandSignalInWorldCoordinates;
    //     commandSignalInWorldCoordinates = ScaleInput(EMGSignal, 0, 1, minSpaceshipAltitude, maxSpaceshipAltitude); // shift origin to bottom of spaceship
    //     return commandSignalInWorldCoordinates;
    // }

    // public void CoinFinalAltitude(float altitude)
    // {
    //     coinFinalAltitude = altitude;
    // }

    // private float ScaleCoinAltitudeByScreenSize()
    // {
    //     Vector2 coinPosition = CalculateGameObjectScaledScreenPosition(0f, coinFinalAltitude + (coinHeight / 2), coinHeight, safeScreenBottomCoordinate, safeScreenTopCoordinate);
    //     coinAltitudeScaledByScreenSize = coinPosition.y;
    //     return coinAltitudeScaledByScreenSize;
    // }

    // private float ScaleCoinPaddedHeightByScreenSize()
    // {
    //     coinHeightScaledByScreenSize = coinPaddedHeight / (safeScreenTopCoordinate - safeScreenBottomCoordinate);
    //     return coinHeightScaledByScreenSize;
    // }

    // private float ScaleSpaceshipAltitudeByScreenSize()
    // {
    //     Vector2 pos = CalculateGameObjectScaledScreenPosition(spaceshipCommandPositionX, spaceshipCommandSignalScaledByMinMaxAltitudeWithHeightOffset, spaceshipHeight, safeScreenBottomCoordinate, safeScreenTopCoordinate);
    //     return pos.y;
    // }

    // private float ScaleSpaceshipAltitudeAtCrashByScreenSize()
    // {
    //     Vector2 pos = CalculateGameObjectScaledScreenPosition(spaceshipCommandPositionX, spaceshipControlSignalAtCrash, spaceshipHeight, safeScreenBottomCoordinate, safeScreenTopCoordinate);
    //     return pos.y;
    // }

    // private float ScaleCurveAmplitudeByScreenSize()
    // {
    //     Vector2 curvePos = CalculateGameObjectScaledScreenPosition(0f, curveAmplitude, 0f, safeScreenBottomCoordinate, safeScreenTopCoordinate);
    //     curveAmplitudeScaledByScreenSize = curvePos.y;
    //     return curveAmplitudeScaledByScreenSize;
    // }

    // public void IncreaseAmplitude()
    // {
    //     a += 10f;
    // }

    // public void DecreaseAmplitude()
    // {
    //     a -= 10f;
    // }

    // public void IncreaseSigma()
    // {
    //     sigma += 0.01f;
    // }

    // public void DecreaseSigma()
    // {
    //     sigma -= 0.01f;
    // }

    // public void IncreaseSpeed()
    // {
    //     muStep += 0.005f;
    // }

    // public void DecreaseSpeed()
    // {
    //     muStep -= 0.005f;
    // }

    // public void IncreaseAmplitudeMaxProbability()
    // {
    //     if (supermaxProbability < 1.0f)
    //     {
    //         supermaxProbability += 0.1f;
    //     }
    // }

    // public void DecreaseAmplitudeMaxProbability()
    // {
    //     if (supermaxProbability > 0.0f)
    //     {
    //         supermaxProbability -= 0.1f;
    //     }
    // }
}
