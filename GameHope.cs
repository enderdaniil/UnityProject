using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameHope : MonoBehaviour
{
    public GameObject playGround;
    public GameObject targetCube;
    private GameObject copyTargetCube;
    private GameObject usingCopy;

    public GameObject timerText;
    private float textTimer;
    private int timeLeft;

    private int amountOfMisses;
    public GameObject missCount;

    public GameObject ruleText;
    private string rules;

    private float targetTimer;

    private float reduceTime;

    private float bigTimer;
    private float spawnTime;

    private float xSize;
    private float ySize;
    private Vector3[,] cellsDistanation;

    private bool start;

    // Start is called before the first frame update
    void Start()
    {
        start = GameStartHope.start;

        bigTimer = 0;
        timeLeft = 60;
        textTimer = 3;

        reduceTime = 0;

        targetTimer = 0;
        amountOfMisses = 0;

        spawnTime = 3;

        Mesh playGroundMesh = playGround.GetComponent<MeshFilter>().mesh;
        xSize = playGroundMesh.bounds.size.x * playGround.transform.localScale.x / 5;
        ySize = playGroundMesh.bounds.size.y * playGround.transform.localScale.y / 5;
        rules = "Для начала игры нажмите на панель перед вами." +
            "                                                                                                               После этого нажимайте на появляющиеся квадраты как можно быстрее. ";

        cellsDistanation = new Vector3[5, 5];
        for (int i = -2; i < 3; i++)
        {
            for (int j = -2; j < 3; j++)
            {
                Vector3 position = new Vector3((i * xSize), (j * ySize), 0);
                position += playGround.transform.position;
                cellsDistanation[i + 2, j + 2] = position;
            }
        }

        copyTargetCube = targetCube;
        copyTargetCube.transform.localScale = new Vector3(xSize, ySize, 0.02f);

        timerText.GetComponent<TextMesh>().text = "";
        ruleText.GetComponent<TextMesh>().text = rules;
        missCount.GetComponent<TextMesh>().text = "";
    }

    // Update is called once per frame
    void Update()
    {
        start = GameStartHope.start;

        missCount.GetComponent<TextMesh>().text = "Кол-во промахов: " + amountOfMisses;
        timerText.GetComponent<TextMesh>().text = "Осталось: " + timeLeft + " сек";

        if (start && timeLeft > 0)
        {
            targetTimer += Time.deltaTime;
            bigTimer += Time.deltaTime;
            reduceTime += Time.deltaTime;

            textTimer += 1 * Time.deltaTime;
            if (textTimer >= 1)
            {
                timeLeft -= 1;
                textTimer = 0;
            }
            if (timeLeft <= 15)
            {
                timerText.GetComponent<TextMesh>().color = Color.red;
            }

            if (GameStartHope.miss == true)
            {
                Debug.Log("Tap = true! Miss");
                GameStartHope.miss = false;
                TappingHope.tap = true;
                amountOfMisses++;
            }

            if (TappingHope.tap == false && targetTimer >= spawnTime)
            {
                Debug.Log("Tap = true! target timer");
                TappingHope.tap = true;
                amountOfMisses++;
                Destroy(usingCopy);
            }

            if (Tapping.tap)
            {
                Debug.Log("New Cube!");
                Destroy(usingCopy);
                int i = Random.Range(0, 5);
                int j = Random.Range(0, 5);
                usingCopy = (GameObject)(Instantiate(copyTargetCube, cellsDistanation[i, j], Quaternion.identity));
                targetTimer = 0;
            }

            if (reduceTime >= 2.5 && spawnTime >= 0.71)
            {
                spawnTime -= 0.2f;
                reduceTime = 0;
            }
        }
        else if (timeLeft <= 0)
        {
            Destroy(usingCopy);
        }
    }
}
