import requests
import json

url = 'https://api.checkwx.com/metar/KSHN/decoded'

url = 'https://api.checkwx.com/metar/lat/47.24216/lon/-123.14186/decoded'
url = 'https://api.checkwx.com/metar/KAVL/decoded'
response = requests.request('GET', url, headers= {'X-API-Key': ''})
print(json.dumps(json.loads(response.text), indent=4))