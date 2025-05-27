#!/bin/bash

MAX_MB=90
TOTAL_SIZE=0
FILES=()
BATCH=1

echo "🚀 자동 분할 커밋 시작"

# 새 파일 목록 가져오기 (추적 안 된 파일만)
FILES_TO_COMMIT=$(git ls-files --others --exclude-standard)

for FILE in $FILES_TO_COMMIT; do
  # 파일이 실제 존재하는지 확인
  if [[ ! -f "$FILE" ]]; then
    continue
  fi

  # 용량 계산 (MB 단위)
  FILESIZE=$(du -m "$FILE" | cut -f1)

  # 단일 파일 크기 제한 넘는 건 경고 후 무시
  if [[ $FILESIZE -gt $MAX_MB ]]; then
    echo "⚠️ 단일 파일 용량 초과: $FILE ($FILESIZE MB) → 스킵"
    continue
  fi

  # 누적 용량 체크
  if [[ $((TOTAL_SIZE + FILESIZE)) -gt $MAX_MB ]]; then
    echo "📦 커밋 $BATCH: ${#FILES[@]}개 파일, 총 ${TOTAL_SIZE}MB"
    git add "${FILES[@]}"
    git commit -m "Auto commit batch $BATCH (${TOTAL_SIZE}MB)"
    git push origin main
    FILES=()
    TOTAL_SIZE=0
    ((BATCH++))
  fi

  FILES+=("$FILE")
  TOTAL_SIZE=$((TOTAL_SIZE + FILESIZE))
done

# 마지막 커밋
if [[ ${#FILES[@]} -gt 0 ]]; then
  echo "📦 커밋 $BATCH: ${#FILES[@]}개 파일, 총 ${TOTAL_SIZE}MB"
  git add "${FILES[@]}"
  git commit -m "Auto commit batch $BATCH (${TOTAL_SIZE}MB)"
  git push origin main
fi

echo "✅ 완료!"
