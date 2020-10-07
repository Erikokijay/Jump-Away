using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LangChanger : MonoBehaviour
{
    // Start is called before the first frame update
    //ENG(0), FRA(1), ITA(2), ISP(3), CHI(4), RUS(5), IND(6)
    public Text[] texts;
    public Font latin, chi, rus, ind;
    public string[,] langs = {
        {"Choose", "Choosed", "Settings", "Characters", "Pause", "Start", "ENG", "Language"},
        {"Choisir", "Choisie", "Reglages", "Personnages", "Pause", "Start", "FRA", "Language"},
        {"Choose", "Choosed", "Settings", "Characters", "Pause", "Start", "ITA", "Language"},
        {"Choose", "Choosed", "Settings", "Characters", "Pause", "Start", "ESP", "Language"},
        {"Choose", "Choosed", "Settings", "Characters", "Pause", "Start", "中文", "Language"},
        {"Выбрать", "Выбран", "Настройки", "Персонажи", "Пауза", "Start", "РУС", "Язык"},
        {"Choose", "Choosed", "Settings", "Characters", "Pause", "Start", "हिन्दी", "Language"}
    };

    public void ChangeLang(int lang){
        for(int i=0; i<texts.Length-1; i++){
            if(lang<=3){
                texts[i].GetComponent<Text>().font = latin;
            } else if(lang==4){
                texts[i].GetComponent<Text>().font = chi;
            } else if(lang==5){
                texts[i].GetComponent<Text>().font = rus;
            } else if(lang==6){
                texts[i].GetComponent<Text>().font = ind;
            }
            texts[i].GetComponent<Text>().text = langs[lang, i];
        }
    }
}
