import time
import cv2
import matplotlib.pyplot as plt
import mysql.connector
import face_recognition
import numpy as np
import base64
from deepface import DeepFace
import sys

def main(name='mashood',course='AAI',date='07/06/2021',start_time="22:10:56"):

    connection = mysql.connector.connect(host='localhost',
                                                 database='SSDA',
                                                 user='root',
                                                 password='mashood.1')

    sql_fetch_Query ="""SELECT Image,Time FROM images WHERE Name=%s AND Course=%s AND Date=%s AND Start_Time=%s"""
    cursor = connection.cursor()
    cursor.execute(sql_fetch_Query, (name,course,date,start_time,))
    records = cursor.fetchall()
    #print(records)
    print("length of recrds ",len(records))
    sql_insert ="""UPDATE ssda.results SET Emotion=%s WHERE (Id=%s)"""
    i=0
    while i<len(records):
        with open("imageToSave.png", "wb") as fh:
            fh.write(records[i][0])
        mood=''
        
        sql_result_Query ="""SELECT Detection,Id FROM results WHERE Name=%s AND Course=%s AND Date=%s AND Start_Time=%s AND Time=%s"""
        #cursor = connection.cursor()
        cursor.execute(sql_result_Query, (name,course,date,start_time,records[i][1],))
        result = cursor.fetchone()
        results = result[0]
        Id=  result[1]
        print("Time = ", records[i][1])
        print("Results = ",results)
        print ("Id = ",Id)
        if results =='True':
            try:
                obj = DeepFace.analyze(img_path = "imageToSave.png", actions = ['emotion'])
                print(obj['emotion'])
                high=0 # mood probability 
                for key in obj['emotion']:
                    val = obj['emotion'][key]
                    if val>high:
                        mood= key
                        high=val
                        
            except:
                mood="0"
            print(mood)
            #cursor = connection.cursor()
            try:
                cursor.execute(sql_insert, (mood,Id,))
                connection.commit()
                print("added")
            except:
                print("error")
        
        #print(results)
        i=i+1   
arguments = sys.argv 
name=arguments[1]
course=arguments[2]
date=arguments[3]
start_time=arguments[4]
#main()
main(name,course,date,start_time)