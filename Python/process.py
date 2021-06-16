import time
import cv2
import matplotlib.pyplot as plt
import mysql.connector
import face_recognition
import numpy as np
import base64
from deepface import DeepFace
import sys

def collect_encoding(name='mashood'):
    connection = mysql.connector.connect(host='localhost',
                                                 database='SSDA',
                                                 user='root',
                                                 password='mashood.1')
    
    sql_fetch_Query ="""SELECT Feature FROM Images WHERE User_Name=%s"""
    cursor = connection.cursor()
    cursor.execute(sql_fetch_Query, (name,))
    records = cursor.fetchall()
    #print(records[0][0])
    buffer = records[0][0]
    buffer = buffer.split(',')
    x=[]
    i=0
    while i < (len(buffer)-1):
        x.append(float(buffer[i]))
        i= i +1 
    print("Base Image is fetched ")    
    connection.commit()
    cursor.close()
    connection.close()
    print("MySQL connection is closed")
    return x



def main(name='mashood',course='SQE',date='07/06/2021',start_time="06:45:36",ctime="120"):
    print("Name = ",name)
    print("Course = ",course)
    print("Date = ",date)
    print(start_time)
    print(ctime)

    data = collect_encoding('Mashood')
    
    connection = mysql.connector.connect(host='localhost',
                                                 database='SSDA',
                                                 user='root',
                                                 password='mashood.1')
    
    sql_fetch_Query ="""SELECT Image FROM images WHERE Name=%s AND Course=%s AND Date=%s AND Start_Time=%s AND Time=%s"""
    cursor = connection.cursor()
    cursor.execute(sql_fetch_Query, (name,course,date,start_time,ctime,))
    records = cursor.fetchone()
    print(records)
    print("length of recrds ",len(records))
    sql_insert ="""INSERT INTO results (Name, Course,Date,Detection,Emotion,Start_Time,Time) VALUES (%s, %s, %s, %s,%s,%s,%s)"""
    i=0
    while i<len(records):
        with open("imageToSave.png", "wb") as fh:
            fh.write(records[0])
        mood=''
        results = compare('imageToSave.png',data)
        """
        if results =='True':
            obj = DeepFace.analyze(img_path = "imageToSave.png", actions = ['emotion'])
            print(obj['emotion'])
            high=0 # mood probability 
            for key in obj['emotion']:
                val = obj['emotion'][key]
                if val>high:
                    mood= key
                    high=val
            print(mood)
        """
        cursor.execute(sql_insert, (name,course,date,results,mood,start_time,ctime,))
        print(results)
        i=i+1
    
    connection.commit()
    cursor.close()
    connection.close()
    print("MySQL connection is closed")


def compare(image,data):
    unknown = face_recognition.load_image_file(image)
    if face_recognition.face_encodings(unknown):
        unknown_face_encoding = face_recognition.face_encodings(unknown)[0]
        try:
            if unknown_face_encoding != []:
                results = face_recognition.compare_faces([data], unknown_face_encoding)
                if results[0] == True:
                    return 'True'
                return "False"
            else:
                return "Falsee"
        except:
            print("Failed compare")
    else:
        return 0

arguments = sys.argv 
name=arguments[1]
course=arguments[2]
date=arguments[3]
start_time=arguments[4]
ctime=arguments[5]
#main()
main(name,course,date,start_time,ctime)