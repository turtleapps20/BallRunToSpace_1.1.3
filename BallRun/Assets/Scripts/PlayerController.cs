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
    public InterAdMob interAdMob;//�������
    private int tryCount;//����� �������

    void Start()
    {
        coins = PlayerPrefs.GetInt("coins");//������� ����� ����� ���������� �� �������
        coinsText.text = coins.ToString();
        controller = GetComponent<CharacterController>();
        StartCoroutine(SpeedIncrease());

        tryCount = PlayerPrefs.GetInt("tryCount");//������� ����� ������� ���������� �� �������
    }

    void LateUpdate()
    {
        //�������� �����
        dir.z = speed;
        dir.y += gravity * Time.fixedDeltaTime;
        controller.Move(dir * Time.fixedDeltaTime);

        //������������� �������� ������-�����
        horizontalInput = joystick.Horizontal;
        transform.Translate(Vector3.right * Time.fixedDeltaTime * speed * horizontalInput);

        //��������
        Quaternion rotationX = Quaternion.AngleAxis(rotationPlayer, Vector3.right);
        transform.rotation *= rotationX;
    }

    private IEnumerator SpeedIncrease()//���������� �������� �� 1 ������ 5 ������
    {
        yield return new WaitForSeconds(4);
        if (speed<maxSpeed)//����������� ������������ ��������
        {
            speed += 2;
            StartCoroutine(SpeedIncrease());
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "obstacle")//���� ����� ��������� � �������� � ����� obstacle, ��
        {
            losePanel.SetActive(true);//������ ������������
            gameObject.SetActive(false);//����� ��������
            Instantiate(deadSound, transform.position, Quaternion.identity);//��������������� ����� ������
            Instantiate(explosion, transform.position, Quaternion.identity);//��������������� ������� ������
            int lastRunScore = int.Parse(scoreScript.scoreText.text.ToString());
            PlayerPrefs.SetInt("lastRunScore", lastRunScore);

            tryCount++;//����� ������� �������������
            PlayerPrefs.SetInt("tryCount", tryCount);//����� ������� ����������� � ������
            if(tryCount % 2 == 0)//������ 2 �������
            {
                interAdMob.ShowAd();//������������ �������
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Coin")
        {
            Instantiate(coinEffects, transform.position, Quaternion.identity);//��������������� ������� ����� �����
            Instantiate(coinSound, transform.position, Quaternion.identity);//��������������� ����� ����� �����
            coins++;//���������� �����
            PlayerPrefs.SetInt("coins", coins);//���-�� ����� ����������� � ������
            coinsText.text = coins.ToString();
            Destroy(other.gameObject);//������ ������������
        }
    }
}
