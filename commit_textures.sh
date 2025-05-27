#!/bin/bash

TEXTURE_DIR="Assets/BlueDotStudios/Hospital/Textures"
PREFIXES=$(ls "$TEXTURE_DIR" | grep -oE '^[^_]*' | sort | uniq)

for prefix in $PREFIXES
do
  echo "▶️ 커밋 중: $prefix"#!/bin/bash

TEXTURE_DIR="Assets/BlueDotStudios/Hospital/Textures"
PREFIXES=$(ls "$TEXTURE_DIR" | grep -oE '^[^_]*' | sort | uniq)

for prefix in $PREFIXES
do
  echo "▶️ 커밋 중: $prefix"
  git add "$TEXTURE_DIR/${prefix}"*
  
  if git diff --cached --quiet; then
    echo "  ⚠️ 변경 없음. 스킵"
  else
    git commit -m "Add textures for $prefix"
    git push origin main
  fi
done

echo "✅ 모두 완료!"
  git add "$TEXTURE_DIR/${prefix}"*
  
  if git diff --cached --quiet; then
    echo
  else
    git commit -m "Add textures for $prefix"
    git push origin main
  fi
done

echo 
