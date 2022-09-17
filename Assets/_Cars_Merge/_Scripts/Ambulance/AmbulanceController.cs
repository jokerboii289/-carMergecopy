using System.Collections;
using System.Collections.Generic;
using _Cars_Merge._Scripts.ControllerRelated;
using _Cars_Merge._Scripts.ElementRelated;
using _Draw_Copy._Scripts.ControllerRelated;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmbulanceController : MonoBehaviour
{
    public static AmbulanceController instance;

    public List<GameObject> cars;
    public List<Sprite> carSprites;
    [Range(1, 6)] public int difficulty;
    public Image nextCarImg, targetCarImg;

    [HideInInspector] public Vector3 dir;

    [HideInInspector] public GameObject currentCar;
    public TextMeshProUGUI targetNumText, nextCarNumText;

    public Dictionary<int, GameObject> carNumPair = new Dictionary<int, GameObject>();
    public Dictionary<int, Sprite> carImgPair = new Dictionary<int, Sprite>();
    public GameObject mergeFx;

    [Header("no of ambulance to evacuate")]
    public int TotalNoOfAmbulance;
    private int counter;
    private int inputCounter;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        inputCounter = 0;
        counter = 0;
        int value = 2;
        currentCar = GetCurrentCar();
        for (int i = 0; i < cars.Count; i++)
        {
            carNumPair.Add(value, cars[i]);
            carImgPair.Add(value, carSprites[i]);
            value *= 2;
        }
        SetCarInUI();
    }

    //By arrow click
    public void SpawnCar(Transform refObj)
    {
        dir = refObj.forward;
        GameObject newcar = Instantiate(currentCar, refObj.position, refObj.rotation);
        newcar.GetComponent<CarElement>().engineSmoke.SetActive(true);
        currentCar = GetCurrentCar();
        SetCarInUI();
    }

    void SetCarInUI()
    {
        int carNum = currentCar.GetComponent<CarElement>().num;
        nextCarNumText.text = carNum.ToString();
        nextCarImg.sprite = carImgPair[carNum];
        nextCarImg.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-107, 181f), 0.25f).From();
    }
    public void SetupMergedCar(Transform collidingCar, Transform targetCar, int num)
    {
        print(num);
        if (num > 128) return;
        dir = targetCar.forward;

        CarElement carElement = targetCar.GetComponent<CarElement>();
        Vector3 tilePos = carElement.tileOccupied.position;

        //calculating spawn rotation
        if (!collidingCar.GetComponent<CarElement>().tileOccupied)
            collidingCar.GetComponent<CarElement>().tileOccupied = carElement.tileOccupied;
        Vector3 spawnDir = targetCar.GetComponent<CarElement>().tileOccupied.position - collidingCar.GetComponent<CarElement>().tileOccupied.position;
        Vector3 spawnAngle = new Vector3();
        //calculate offset
        Vector3 offset = Vector3.zero;
        spawnAngle = collidingCar.eulerAngles + Vector3.up * 180; 
        if (spawnDir.z == 0)
        {
            if (num > 16)
                offset = new Vector3(3, 0, 0);
        }
        else if (spawnAngle.x == 0)
        {
            if (num > 16) offset = new Vector3(0, 0,-3);
        }

        var InstantiatePos = new Vector3(tilePos.x, targetCar.position.y, tilePos.z);
        GameObject newcar = Instantiate(carNumPair[num], InstantiatePos + offset, Quaternion.Euler(spawnAngle));
        newcar.GetComponent<Collider>().enabled = false;
        newcar.GetComponent<CarMovementElement>().enabled = false;
        SoundsController.instance.PlaySound(SoundsController.instance.merge);

        float origY = newcar.transform.position.y;
        newcar.transform.DOLocalRotate(Vector3.one * 60, 0.5f).From();
        newcar.transform.DOScale(Vector3.one * 0.3f, 0.5f).From();
        newcar.transform.DOMoveY(newcar.transform.position.y + 2.5f, 0.5f).OnComplete(() =>
        {
            newcar.transform.DOMoveY(origY, 0.5f);
            newcar.GetComponent<Collider>().enabled = true;
            newcar.GetComponent<CarMovementElement>().enabled = true;
        });
        GameObject fx = Instantiate(mergeFx, InstantiatePos + offset, Quaternion.identity); //smoke
        fx.transform.parent = newcar.transform;
        targetCar.gameObject.SetActive(false);
    }
    public GameObject GetCurrentCar()
    {
        return cars[Random.Range(0, difficulty)];
    }

    IEnumerator TargetCarFx(GameObject newcar)
    {
        newcar.GetComponent<CarMovementElement>().enabled = false;
        newcar.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(0.65f);
        Material finalCarMat = newcar.transform.GetChild(0).GetComponent<Renderer>().material;
        Color origColor = finalCarMat.color;
        var sequence = DOTween.Sequence();
        sequence.Append(finalCarMat.DOColor(Color.gray, 0.5f));
        sequence.Append(finalCarMat.DOColor(origColor, 0.5f));
        sequence.Append(finalCarMat.DOColor(Color.gray, 0.5f));
        sequence.Append(finalCarMat.DOColor(origColor, 0.5f));

        MainController.instance.SetActionType(GameState.Levelwin);
    }

    public void AmbulanceCounter()
    {
        counter++;
        if (counter == TotalNoOfAmbulance)
            MainController.instance.SetActionType(GameState.Levelwin);
    }

    public void InputModifier()
    {
        inputCounter++;
        if (inputCounter > 1)
        {
            MainController.instance.SetActionType(GameState.Input);
            inputCounter=0;
        }
    }
}

