#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
一鍵匯出指定工作表（免打參數版）
--------------------------------------------------
把下面「設定區」改成你的預設值，直接執行此檔即可完成匯出。

依賴套件：
    pip install pandas openpyxl
"""

from __future__ import annotations
from pathlib import Path
import re
import sys
import pandas as pd
import os

# ======== 設定區（改這裡就好） ========
DATE = "202509"                 # 例如：202508
REGION = "R24"                  # 例如：R23
BASE_DIR = os.path.join(r"C:\TWSC_Toolset\TWN\SourceData", DATE)
INPUT_BASENAME = "TWN Speed cam update.xlsx"
OUTDIR = ""                  # 輸出資料夾

# 要匯出的工作表清單（依序處理）
SHEETS = [
    "Taiwan科技執法",
    "Taiwan固定式測速",
    "Taiwan移動式",
    "Taiwan區間測速",
    "固定式合併科技執法",
    "機車",
    "Taiwan常事故點",
]

# 每個工作表對應的輸出檔名樣板（可用 {date}、{region}）
NAME_MAP = {
    "Taiwan科技執法": "tech_{region}.xlsx",
    "Taiwan固定式測速": "fixed_{date}.xlsx",
    "Taiwan移動式": "mobile_{date}.xlsx",
    "Taiwan區間測速": "average_{date}.xlsx",
    "固定式合併科技執法": "combine_{date}.xlsx",
    "機車": "motorcycle_{date}.xlsx",
    "Taiwan常事故點": "popular_{date}.xlsx",
}
# =====================================


def sanitize_filename(name: str) -> str:
    name = re.sub(r'[\\/:*?"<>|]', "_", name)  # windows-illegal
    name = re.sub(r"\s+", " ", name).strip()
    return name or "sheet"


def export_selected_sheets(
    input_path: Path,
    sheets: list[str],
    name_map: dict[str, str],
    placeholders: dict[str, str],
    out_dir: Path,
) -> None:
    out_dir.mkdir(parents=True, exist_ok=True)

    if not input_path.exists():
        print(f"[錯誤] 找不到輸入檔：{input_path}", file=sys.stderr)
        sys.exit(1)

    xls = pd.ExcelFile(input_path, engine="openpyxl")
    existing = set(xls.sheet_names)

    exported = 0
    for s in sheets:
        if s not in existing:
            print(f'[跳過] 找不到工作表: "{s}"（可先確認表名是否一致）', file=sys.stderr)
            continue

        df = pd.read_excel(xls, sheet_name=s)

        template = name_map.get(s, f"{s}.xlsx")
        try:
            fname = template.format(**placeholders)
        except KeyError as e:
            missing = e.args[0]
            print(f'[錯誤] 檔名樣板 "{template}" 缺少 placeholder: {{{missing}}}', file=sys.stderr)
            sys.exit(2)

        out_path = out_dir / sanitize_filename(fname)

        with pd.ExcelWriter(out_path, engine="openpyxl") as writer:
            sheet_name = s[:31]  # Excel 表名最多 31 字
            df.to_excel(writer, index=False, sheet_name=sheet_name)

        exported += 1
        print(f'[OK] {s} -> {out_path.name}')

    if exported == 0:
        print("[警告] 沒有任何工作表被匯出。", file=sys.stderr)
    else:
        print(f"[完成] 共輸出 {exported} 個檔案到：{out_dir.resolve()}")


def main():
    base_dir = Path(BASE_DIR)
    input_path = Path(os.path.join(BASE_DIR, INPUT_BASENAME))
    out_dir = Path(os.path.join(BASE_DIR, OUTDIR)) if OUTDIR else base_dir

    placeholders = {"date": DATE, "region": REGION}

    export_selected_sheets(
        input_path=input_path,
        sheets=SHEETS,
        name_map=NAME_MAP,
        placeholders=placeholders,
        out_dir=out_dir,
    )


if __name__ == "__main__":
    main()
