#!/bin/bash

project_name="SkyBro"
project_location="src/SkyBro"
skill_id="amzn1.ask.skill.864910de-f456-430f-a34d-d945e9a05378"
locale="en-US"
file="skill/interactionModels/custom/en-US.json"

while [[ "$#" -gt 0 ]]; do
    case $1 in
        --project-name) project_name="$2"; shift ;;
        --project-location) project_location="$2"; shift ;;
        --skill-id) skill_id="$2"; shift ;;
        --locale) locale="$2"; shift ;;
        --file) file="$2"; shift ;;
        *) echo "Unknown parameter passed: $1"; exit 1 ;;
    esac
    shift
done

echo "Using the following configuration:"
echo "Project Name: $project_name"
echo "Project Location: $project_location"
echo "Skill ID: $skill_id"
echo "Locale: $locale"
echo "File: $file"

dotnet lambda deploy-function "$project_name" --project-location "$project_location"

ask smapi update-interaction-model \
    --skill-id "$skill_id" \
    --locale "$locale" \
    --file "$file"

ask smapi build-skill \
    --skill-id "$skill_id"