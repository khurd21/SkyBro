import requests
import json
import boto3

ssm = boto3.client('ssm')
response = ssm.get_parameter(Name='/SkyBro/CheckWXClient/ApiKey', WithDecryption=True)
api_key = response['Parameter']['Value']

url = 'https://api.checkwx.com/metar/KSHN/decoded'
url = 'https://api.checkwx.com/metar/lat/47.24216/lon/-123.14186/decoded'
url = 'https://api.checkwx.com/metar/KRDU/decoded'
response = requests.request('GET', url, headers= {'X-API-Key': api_key})
print(json.dumps(json.loads(response.text), indent=4))