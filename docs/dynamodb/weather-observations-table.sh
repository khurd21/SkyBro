## For use to setup a DynamoDB table for weather observations ##

# Require aws cli be installed

if ! command -v aws >/dev/null 2>&1; then
  echo "aws cli not found"
  exit 1
fi

# Create table from weather-observations-table.json

# Name of the file containing the table definition
table_definition_file="weather-observations-table.json"
endpoint_url="http://localhost:8000"
region="us-east-2"

# `--local` - if this is passed in, the table will be created in the local DynamoDB instance
# `--live` - if this is passed in, the table will be created in the live DynamoDB instance

if [ "$1" = "--local" ]; then
  aws dynamodb create-table --endpoint-url ${endpoint_url} --cli-input-json file://${table_definition_file}
elif [ "$1" = "--live" ]; then
  aws dynamodb create-table --cli-input-json file://${table_definition_file} --region ${region}
else
  echo "Please specify either --local or --live"
  exit 1
fi
