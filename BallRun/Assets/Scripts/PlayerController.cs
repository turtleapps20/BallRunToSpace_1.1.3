using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 dir;
    [SerializeField] private float speed;
    private float maxSpeed = 70;
    [SerializeField] private float gravity;
    private float horizontalInput;
    public float rotationPlayer;

    [Header("UI")]
    public Joystick joystick;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject explosion;
    [SerializeField] private Score scoreScript;

    [Header("Coin")]
    [SerializeField] private int coins;
    [SerializeField] private Text coinsText;
    [SerializeField] private GameObject coinEffects;

    [Header("Sound")]
    [SerializeField] private GameObject coinSound;
    [SerializeField] private GameObject deadSound;

    [Header("Ads")]
    public InterAdMob interAdMob;//реклама
    private int tryCount;//число попыток

    void Start()
    {
        coins = PlayerPrefs.GetInt("coins");//текущее число монет вызывается из реестра
        coinsText.text = coins.ToString();
        controller = GetComponent<CharacterController>();
        StartCoroutine(SpeedIncrease());

        tryCount = PlayerPrefs.GetInt("tryCount");//текущее число попыток вызывается из реестра
    }

    void LateUpdate()
    {
        //движение вперёд
        dir.z = speed;
        dir.y += gravity * Time.fixedDeltaTime;
        controller.Move(dir * Time.fixedDeltaTime);

        //осуществление движения вправо-влево
        horizontalInput = joystick.Horizontal;
        transform.Translate(Vector3.right * Time.fixedDeltaTime * speed * horizontalInput);

        //вращение
        Quaternion rotationX = Quaternion.AngleAxis(rotationPlayer, Vector3.right);
        transform.rotation *= rotationX;
    }

    private IEnumerator SpeedIncrease()//увеличение скорости на 1 каждые 5 секунд
    {
        yield return new WaitForSeconds(4);
        if (speed<maxSpeed)//ограничение максимальной скорости
        {
            speed += 2;
            StartCoroutine(SpeedIncrease());
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "obstacle")//если игрок столкнётся с объектом с тегом obstacle, то
        {
            losePanel.SetActive(true);//панель активируется
            gameObject.SetActive(false);//игрок исчезает
            Instantiate(deadSound, transform.position, Quaternion.identity);//воспроизведение звука взрыва
            Instantiate(explosion, transform.position, Quaternion.identity);//воспроизведение эффекта взрыва
            int lastRunScore = int.Parse(scoreScript.scoreText.text.ToString());
            PlayerPrefs.SetInt("lastRunScore", lastRunScore);

            tryCount++;//число попыток увеличивается
            PlayerPrefs.SetInt("tryCount", tryCount);//число попыток сохраняется в реестр
            if(tryCount % 2 == 0)//каждые 2 попытки
            {
                interAdMob.ShowAd();//показывается реклама
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Coin")
        {
            Instantiate(coinEffects, transform.position, Quaternion.identity);//воспроизведение эффекта сбора монет
            Instantiate(coinSound, transform.position, Quaternion.identity);//воспроизведение звука сбора монет
            coins++;//увеличение монет
            PlayerPrefs.SetInt("coins", coins);//кол-во монет сохраняется в реестр
            coinsText.text = coins.ToString();
            Destroy(other.gameObject);//монета уничтожается
        }
    }
}
