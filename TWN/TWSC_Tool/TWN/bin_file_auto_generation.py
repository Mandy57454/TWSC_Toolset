import os
import shutil
import pandas as pd
import pyexcel as p
import json
import argparse
import win32com.client

def load_settings(file_path):
    with open(file_path, 'r') as file:
        settings = json.load(file)
    return settings

# 创建 ArgumentParser 对象
parser = argparse.ArgumentParser(description='Load settings from a JSON file.')

# 添加命令行参数
parser.add_argument('settings_file', type=str, help='Path to the settings file.')

# 解析命令行参数
args = parser.parse_args()

# 读取设置文件
settings = load_settings(args.settings_file)

# 将设置分配给变量
build_date = settings['build_date']
build_date_year = settings['build_date_year']
data_ver = settings['data_ver']
build_R01_Only = settings['build_R01_Only']
build_R01_SEA = settings['build_R01_SEA']
build_R12 = settings['build_R12']
build_R21 = settings['build_R21']
build_R22 = settings['build_R22']
build_R23 = settings['build_R23']
build_R24 = settings['build_R24']
build_R41 = settings['build_R41']
build_R99 = settings['build_R99']
r01_388_path = settings['r01_388_path']
r01_only_lw_path = settings['r01_only_lw_path']
r01_tw_sea_lw_path = settings['r01_tw_sea_lw_path']
r12_path = settings['r12_path']
r21_path = settings['r21_path']
r22_path = settings['r22_path']
r23_path = settings['r23_path']
r24_path = settings['r24_path']
r41_path = settings['r41_path']
r99_path = settings['r99_path']
tech_ver = settings['tech_ver']
SEA_ver = settings['SEA_ver']
motorcycle_ver = settings['motorcycle_ver']
output_path_folder = settings['output_path_folder']

########################################################################################################################
out_path = os.path.join(output_path_folder, data_ver)
if os.path.exists(out_path):
    shutil.rmtree(out_path)

# make dir
def make_dir():
    os.makedirs(out_path)
    os.makedirs(os.path.join(out_path, 'Release'))
    os.makedirs(os.path.join(out_path, 'Release', 'R01'))
    os.makedirs(os.path.join(out_path, 'Release', 'R12'))
    os.makedirs(os.path.join(out_path, 'Release', 'R21'))
    os.makedirs(os.path.join(out_path, 'Release', 'R22'))
    os.makedirs(os.path.join(out_path, 'Release', 'R23'))
    os.makedirs(os.path.join(out_path, 'Release', 'R24'))
    os.makedirs(os.path.join(out_path, 'Release', 'R41'))
    os.makedirs(os.path.join(out_path, 'Release', 'R99'))

########################################################################################################################
    # R01
    os.makedirs(os.path.join(out_path, 'Release', 'R01', f'{build_date_year}_v{build_R01_Only}TWOnly')) # 20241223_v90TWOnly
    os.makedirs(os.path.join(out_path, 'Release', 'R01', f'{build_date_year}_v{build_R01_Only}TWOnly', '388'))
    os.makedirs(os.path.join(out_path, 'Release', 'R01', f'{build_date_year}_v{build_R01_Only}TWOnly',
                             f'LW_ABBY_AMY-V{build_R01_Only}.{build_date}.01.01-TWN'))
    os.makedirs(os.path.join(out_path, 'Release', 'R01', f'{build_date_year}_v{build_R01_Only}TWOnly', 'R28'))
    os.makedirs(os.path.join(out_path, 'Release', 'R01', f'{build_date_year}_v{build_R01_Only}TWOnly', 'R52'))
    os.makedirs(os.path.join(out_path, 'Release', 'R01', f'{build_date_year}_v{build_R01_Only}TWOnly', 'R58'))
    os.makedirs(os.path.join(out_path, 'Release', 'R01', f'{build_date_year}_v{build_R01_Only}TWOnly', 'R60R62'))
    os.makedirs(os.path.join(out_path, 'Release', 'R01', f'{build_date_year}_v{build_R01_SEA}TWSEAnoTT')) # 20241223_v93TWSEAnoTT
    os.makedirs(os.path.join(out_path, 'Release', 'R01', f'{build_date_year}_v{build_R01_SEA}TWSEAnoTT',
                             f'LW_ABBY_AMY-V{build_R01_SEA}.{build_date}.01.01-ROW'))
    # R12
    os.makedirs(os.path.join(out_path, 'Release', 'R12', f'LW_ABBY_AMY-V{build_R12}.{build_date}.01.12-ROW'))
    # R21
    os.makedirs(os.path.join(out_path, 'Release', 'R21', f'Hector_v{build_R21}.{build_date}.01.21'))
    # R22
    os.makedirs(os.path.join(out_path, 'Release', 'R22', f'Hector_v{build_R22}.{build_date}.01.22'))
    # R23
    os.makedirs(os.path.join(out_path, 'Release', 'R23', f'V{build_R23}.{build_date}.01.23-ROW'))
    # R24
    os.makedirs(os.path.join(out_path, 'Release', 'R24', f'V{build_R24}.{build_date}.01.24-ROW'))
    # R41
    os.makedirs(os.path.join(out_path, 'Release', 'R41', f'York-V{build_R41}_v{build_date_year}-ROW'))
    # R99
    os.makedirs(os.path.join(out_path, 'Release', 'R99', f'V{build_R99}.{build_date}.01.99-ROW'))
########################################################################################################################

def repair_xls_format(input_path, output_path):
    excel = win32com.client.Dispatch("Excel.Application")
    excel.DisplayAlerts = False
    wb = excel.Workbooks.Open(input_path)
    wb.SaveAs(output_path, FileFormat=56)  # 56 = xlExcel8 = .xls 格式
    wb.Close()
    excel.Quit()
########################################################################################################################

def delete_file(file_path):
    """刪除指定的檔案，若不存在則顯示提示。"""
    if os.path.exists(file_path):
        os.remove(file_path)
        print(f"檔案已刪除: {file_path}")
    else:
        print(f"找不到檔案: {file_path}")
########################################################################################################################
# 388 -> fixed+combine (type 3改成6, 18 & 19改成1)
def r01_388(fixed, combine):
    print('start process r01_388')
    
    r01_388_data = p.get_book(file_name=r01_388_path)
    out_r01_388 = os.path.join(out_path, 'Release', 'R01', f'{build_date_year}_v{build_R01_Only}TWOnly', '388',
                               f'TW_{data_ver}_MioSpeedCam-388.xls')
    out_temp = os.path.join(out_path, 'Release', 'R01', f'{build_date_year}_v{build_R01_Only}TWOnly', '388',
                               f'temp.xls')
    # 修改 'note' 工作表
    if 'note' in r01_388_data.sheet_names():
        sheet = r01_388_data['note']
        # pyexcel 是1-based index，所以1000行是 1000
        sheet[999, 5] = build_R01_Only  # pandas 是 0-based index 所以 1000 行是 999

    # 修改 'data' 工作表
    if 'data' in r01_388_data.sheet_names():
        sheet = r01_388_data['data']
        number_of_rows = sheet.number_of_rows()
        # 只保留第一列
        for i in range(number_of_rows - 1, 0, -1):
            del sheet.row[i]
        # 將fixed_data中要加入的提出來
        fixed_extracted_columns = fixed.iloc[:, [0, 1, 2, 3, 4, 5, 6]]
        fixed_extracted_data = fixed_extracted_columns.values.tolist()
        for row in fixed_extracted_data:
            sheet.row += [row]
        # print(sheet)
        # 將combine_data中要加入的提出來
        combine_extracted_columns = combine.iloc[:, [0, 1, 2, 3, 4, 5, 6]]
        combine_extracted_data = combine_extracted_columns.values.tolist()
        for row in combine_extracted_data:
            sheet.row += [row]
        for i in range(1, sheet.number_of_rows()):  # 3 改成 6
            sheet[i, 1] = round(sheet[i, 1], 6)  # 座標取小數點後6位
            sheet[i, 2] = round(sheet[i, 2], 6)  # 座標取小數點後6位
            if sheet[i, 5] == 3:
                sheet[i, 5] = 6
            if sheet[i, 5] == 18 or sheet[i, 5] == 19:  # 18 & 19 改成 1
                sheet[i, 5] = 1
            sheet[i, 7] = int(data_ver)  # Version
        r01_388_data.save_as(out_temp)
        repair_xls_format(out_temp, out_r01_388);
        delete_file(out_temp);
    else:
        print('data not found')
    print('end process r01_388')
########################################################################################################################


# TWOnly LW_ABBY_AMY -> fixed+combine (18 & 19改成1)
def r01_only_lw(fixed, combine):
    print('start process r01_only_lw')

    r01_only_lw_data = p.get_book(file_name=r01_only_lw_path)
    out_r01_only_lw = os.path.join(out_path, 'Release', 'R01', f'{build_date_year}_v{build_R01_Only}TWOnly',
                                   f'LW_ABBY_AMY-V{build_R01_Only}.{build_date}.01.01-TWN',
                                   f'TW_{data_ver}_MioSpeedCam-newtype_v{build_R01_Only}.xls')
    out_temp = os.path.join(out_path, 'Release', 'R01', f'{build_date_year}_v{build_R01_Only}TWOnly',
                                   f'LW_ABBY_AMY-V{build_R01_Only}.{build_date}.01.01-TWN',
                                   f'temp.xls')
    # 修改 'note' 工作表
    if 'note' in r01_only_lw_data.sheet_names():
        sheet = r01_only_lw_data['note']
        sheet[999, 5] = build_R01_Only

    # 修改 'data' 工作表
    if 'data' in r01_only_lw_data.sheet_names():
        sheet = r01_only_lw_data['data']
        number_of_rows = sheet.number_of_rows()
        # 只保留第一列
        for i in range(number_of_rows - 1, 0, -1):
            del sheet.row[i]
        # 將fixed_data中要加入的提出來
        # fixed
        fixed_extracted_columns = fixed.iloc[:, [0, 1, 2, 3, 4, 5, 6]]
        fixed_extracted_data = fixed_extracted_columns.values.tolist()
        for row in fixed_extracted_data:
            sheet.row += [row]
        # 將combine_data中要加入的提出來
        # combine
        combine_extracted_columns = combine.iloc[:, [0, 1, 2, 3, 4, 5, 6]]
        combine_extracted_data = combine_extracted_columns.values.tolist()
        for row in combine_extracted_data:
            sheet.row += [row]
        # 3 改成 6, 18 & 19 改成 1
        for i in range(1, sheet.number_of_rows()):
            sheet[i, 1] = round(sheet[i, 1], 6)  # 座標取小數點後6位
            sheet[i, 2] = round(sheet[i, 2], 6)  # 座標取小數點後6位
            if sheet[i, 5] == 18 or sheet[i, 5] == 19:  # 18 & 19改成1
                sheet[i, 5] = 1
            sheet[i, 7] = int(data_ver)  # Version
        r01_only_lw_data.save_as(out_temp)
        repair_xls_format(out_temp, out_r01_only_lw);
        delete_file(out_temp);
        
    else:
        print('data not found')
    print('end process r01_only_lw')
########################################################################################################################


# R28,R52, R60R62, R58 (是使用r01_only_lw 的 data, 所以不能單獨使用)
def r28_r52_r60r62_r58():
    print('start process r28_r52_r60r62_r58')
    r28_r52_r60r62_r58_path = os.path.join(out_path, 'Release', 'R01', f'{build_date_year}_v{build_R01_Only}TWOnly',
                                           f'LW_ABBY_AMY-V{build_R01_Only}.{build_date}.01.01-TWN',
                                           f'TW_{data_ver}_MioSpeedCam-newtype_v{build_R01_Only}.xls')
    r28_r52_r60r62_r58_data = p.get_book(file_name=r28_r52_r60r62_r58_path)
    out_r28 = os.path.join(out_path, 'Release', 'R01', f'{build_date_year}_v{build_R01_Only}TWOnly',
                           'R28', f'TW_{data_ver}_MioSpeedCam-newtype_v{build_R01_Only}.csv')
    out_r52 = os.path.join(out_path, 'Release', 'R01', f'{build_date_year}_v{build_R01_Only}TWOnly',
                           'R52', f'TW_{data_ver}_MioSpeedCam-newtype_v{build_R01_Only}.csv')
    out_r60r62 = os.path.join(out_path, 'Release', 'R01', f'{build_date_year}_v{build_R01_Only}TWOnly',
                              'R60R62', f'TW_{data_ver}_MioSpeedCam-newtype_v{build_R01_Only}.csv')
    out_r58 = os.path.join(out_path, 'Release', 'R01', f'{build_date_year}_v{build_R01_Only}TWOnly',
                           'R58', f'TW_{data_ver}_MioSpeedCam-newtype_v{build_R01_Only}.csv')
    if 'data' in r28_r52_r60r62_r58_data.sheet_names():
        sheet = r28_r52_r60r62_r58_data['data']
        new_row = pd.DataFrame({0: [f'TWN_{build_R01_Only}']})
        df = pd.DataFrame(sheet)
        df = pd.concat([df.iloc[:1], new_row, df.iloc[1:]]).reset_index(drop=True)
        df = df.drop(columns=[7, 8])
        cols = df.columns.tolist()
        cols[5], cols[6] = cols[6], cols[5]
        df = df[cols]
        # print(df)
        df.to_csv(out_r28, index=False, header=False, encoding='utf-8-sig')
        df.to_csv(out_r52, index=False, header=False, encoding='utf-8-sig')
        df.to_csv(out_r60r62, index=False, header=False, encoding='utf-8-sig')
        df.to_csv(out_r58, index=False, header=False, encoding='utf-8-sig')
    else:
        print('data not found')
    print('end process r28_r52_r60r62_r58')
########################################################################################################################


# TW_SEA LW_ABBY_AMY -> fixed+combine+SEA_20230217 (5 & 6 18 & 19改成1)
def r01_tw_sea_lw(fixed, combine, sea):
    print('start process r01_tw_sea_lw')
    
    r01_tw_sea_lw_data = p.get_book(file_name=r01_tw_sea_lw_path)
    out_r01_tw_sea_lw = os.path.join(out_path, 'Release', 'R01', f'{build_date_year}_v{build_R01_SEA}TWSEAnoTT',
                                     f'LW_ABBY_AMY-V{build_R01_SEA}.{build_date}.01.01-ROW',
                                     f'TW&SEA_{data_ver}_MioSpeedCam-newtype_v{build_R01_SEA}.xls')
    out_temp = os.path.join(out_path, 'Release', 'R01', f'{build_date_year}_v{build_R01_SEA}TWSEAnoTT',
                                     f'LW_ABBY_AMY-V{build_R01_SEA}.{build_date}.01.01-ROW',
                                     f'temp.xls')
    # 修改 'note' 工作表
    if 'note' in r01_tw_sea_lw_data.sheet_names():
        sheet = r01_tw_sea_lw_data['note']
        sheet[999, 5] = build_R01_SEA
    # 修改 'data' 工作表
    if 'data' in r01_tw_sea_lw_data.sheet_names():
        sheet = r01_tw_sea_lw_data['data']
        number_of_rows = sheet.number_of_rows()
        # 只保留第一列
        for i in range(number_of_rows - 1, 0, -1):
            del sheet.row[i]
        # fixed
        fixed_extracted_columns = fixed.iloc[:, [0, 1, 2, 3, 4, 5, 6]]
        fixed_extracted_data = fixed_extracted_columns.values.tolist()
        for row in fixed_extracted_data:
            sheet.row += [row]
        # combine
        combine_extracted_columns = combine.iloc[:, [0, 1, 2, 3, 4, 5, 6]]
        combine_extracted_data = combine_extracted_columns.values.tolist()
        for row in combine_extracted_data:
            sheet.row += [row]
        count = 1
        for i in range(1, sheet.number_of_rows()):
            sheet[i, 1] = round(sheet[i, 1], 6)  # 座標取小數點後6位
            sheet[i, 2] = round(sheet[i, 2], 6)  # 座標取小數點後6位
            if sheet[i, 5] == 18 or sheet[i, 5] == 19:  # 18 & 19改成1
                sheet[i, 5] = 1
            sheet[i, 7] = int(data_ver)  # Version
            sheet[i, 8] = '固定式'  # Source
            count += 1
        # sea
        for i in range(1, sea.number_of_rows()):
            sheet.row += [sea.row[i][0:8]]
        for i in range(count, sheet.number_of_rows()):
            sheet[i, 1] = round(sheet[i, 1], 6)  # 座標取小數點後6位
            sheet[i, 2] = round(sheet[i, 2], 6)  # 座標取小數點後6位
            if sheet[i, 5] == 5 or sheet[i, 5] == 6:  # SEA 將 5 & 6 改成 1
                sheet[i, 5] = 1
            sheet[i, 8] = 'SEA'  # Source
        r01_tw_sea_lw_data.save_as(out_temp)
        repair_xls_format(out_temp, out_r01_tw_sea_lw);
        delete_file(out_temp);
    else:
        print('data not found')
    print('end process r01_tw_sea_lw')
########################################################################################################################


# R12 -> fixed+combine+SEA (5 & 6 18 & 19改成1)
def r12(fixed, combine, sea):
    print('start process r12')
    
    r12_data = p.get_book(file_name=r12_path)
    out_r12 = os.path.join(out_path, 'Release', 'R12', f'LW_ABBY_AMY-V{build_R12}.{build_date}.01.12-ROW',
                           f'TW&SEA_{data_ver}_MioSpeedCam-newtype_v{build_R12}.xls')
    out_temp = os.path.join(out_path, 'Release', 'R12', f'LW_ABBY_AMY-V{build_R12}.{build_date}.01.12-ROW',
                           f'temp.xls')
    # 修改 'note' 工作表
    if 'note' in r12_data.sheet_names():
        sheet = r12_data['note']
        sheet[999, 5] = build_R12
    # 修改 'data' 工作表
    if 'data' in r12_data.sheet_names():
        sheet = r12_data['data']
        number_of_rows = sheet.number_of_rows()
        # 只保留第一列
        for i in range(number_of_rows - 1, 0, -1):
            del sheet.row[i]
        # fixed
        fixed_extracted_columns = fixed.iloc[:, [0, 1, 2, 3, 4, 5, 6]]
        fixed_extracted_data = fixed_extracted_columns.values.tolist()
        for row in fixed_extracted_data:
            sheet.row += [row]
        # combine
        combine_extracted_columns = combine.iloc[:, [0, 1, 2, 3, 4, 5, 6]]
        combine_extracted_data = combine_extracted_columns.values.tolist()
        for row in combine_extracted_data:
            sheet.row += [row]
        count = 1
        for i in range(1, sheet.number_of_rows()):
            sheet[i, 1] = round(sheet[i, 1], 6)  # 座標取小數點後6位
            sheet[i, 2] = round(sheet[i, 2], 6)  # 座標取小數點後6位
            if sheet[i, 5] == 18 or sheet[i, 5] == 19:  # 18 & 19改成1
                sheet[i, 5] = 1
            sheet[i, 7] = int(data_ver)  # Version
            sheet[i, 8] = '固定式'  # Source
            count += 1
        # sea
        for i in range(1, sea.number_of_rows()):
            sheet.row += [sea.row[i][0:8]]
        for i in range(count, sheet.number_of_rows()):
            sheet[i, 1] = round(sheet[i, 1], 6)  # 座標取小數點後6位
            sheet[i, 2] = round(sheet[i, 2], 6)  # 座標取小數點後6位
            if sheet[i, 5] == 5 or sheet[i, 5] == 6:  # SEA 將 5 & 6 改成 1
                sheet[i, 5] = 1
            sheet[i, 8] = 'SEA'  # Source
        r12_data.save_as(out_temp)
        repair_xls_format(out_temp, out_r12);
        delete_file(out_temp);
    else:
        print('data not found')
    print('end process r12')
########################################################################################################################


# R21 -> fixed+combine+average (18 & 19 改成 1)
def r21(fixed, combine, average):
    print('start process r21')
    
    r21_data = p.get_book(file_name=r21_path)
    out_r21 = os.path.join(out_path, 'Release', 'R21', f'Hector_v{build_R21}.{build_date}.01.21',
                           f'TW_{data_ver}_MioSpeedCam-newtype_v{build_R21}.xls')
    out_temp = os.path.join(out_path, 'Release', 'R21', f'Hector_v{build_R21}.{build_date}.01.21',
                           f'temp.xls')
    # 修改 'note' 工作表
    if 'note' in r21_data.sheet_names():
        sheet = r21_data['note']
        sheet[999, 5] = build_R21
    # 修改 'data' 工作表
    if 'data' in r21_data.sheet_names():
        sheet = r21_data['data']
        number_of_rows = sheet.number_of_rows()
        # 只保留第一列
        for i in range(number_of_rows - 1, 0, -1):
            del sheet.row[i]
        # average
        average_extracted_columns = average.iloc[:, [0, 1, 2, 3, 4, 5, 6, 11]]
        average_extracted_data = average_extracted_columns.values.tolist()
        for row in average_extracted_data:
            if type(row[0]) is str:
                sheet.row += [row]
            else:
                continue
        for row in average_extracted_data:
            if type(row[0]) is int:
                sheet.row += [row]
            else:
                continue
        count = 1
        for i in range(1, sheet.number_of_rows()):
            sheet[i, 1] = round(sheet[i, 1], 6)  # 座標取小數點後6位
            sheet[i, 2] = round(sheet[i, 2], 6)  # 座標取小數點後6位
            sheet[i, 8] = 0  # state
            sheet[i, 9] = '區間測速'  # Source
            count += 1
        # fixed
        fixed_extracted_columns = fixed.iloc[:, [0, 1, 2, 3, 4, 5, 6]]
        fixed_extracted_data = fixed_extracted_columns.values.tolist()
        for row in fixed_extracted_data:
            sheet.row += [row]
        combine_extracted_columns = combine.iloc[:, [0, 1, 2, 3, 4, 5, 6]]
        combine_extracted_data = combine_extracted_columns.values.tolist()
        for row in combine_extracted_data:
            sheet.row += [row]
        for i in range(count, sheet.number_of_rows()):
            sheet[i, 1] = round(sheet[i, 1], 6)  # 座標取小數點後6位
            sheet[i, 2] = round(sheet[i, 2], 6)  # 座標取小數點後6位
            if sheet[i, 5] == 18 or sheet[i, 5] == 19:  # 18 & 19改成1
                sheet[i, 5] = 1
            sheet[i, 7] = int(data_ver)  # version
            sheet[i, 8] = 0  # state
            sheet[i, 9] = '固定式'  # Source
        r21_data.save_as(out_temp)
        repair_xls_format(out_temp, out_r21);
        delete_file(out_temp);
    else:
        print('data not found')
    print('end process r21')
########################################################################################################################


# R22 -> fixed+combine+average+sea (5 & 6 18 & 19改成1)
def r22(fixed, combine, average, sea):
    print('start process r22')
    
    r22_data = p.get_book(file_name=r22_path)
    out_r22 = os.path.join(out_path, 'Release', 'R22', f'Hector_v{build_R22}.{build_date}.01.22',
                           f'TW&SEA_{data_ver}_MioSpeedCam-newtype_v{build_R22}.xls')
    out_temp = os.path.join(out_path, 'Release', 'R22', f'Hector_v{build_R22}.{build_date}.01.22',
                           f'temp.xls')
    # 修改 'note' 工作表
    if 'note' in r22_data.sheet_names():
        sheet = r22_data['note']
        sheet[999, 5] = build_R22
    # 修改 'data' 工作表
    if 'data' in r22_data.sheet_names():
        sheet = r22_data['data']
        number_of_rows = sheet.number_of_rows()
        # 只保留第一列
        for i in range(number_of_rows - 1, 0, -1):
            del sheet.row[i]
        # average
        average_extracted_columns = average.iloc[:, [0, 1, 2, 3, 4, 5, 6, 11]]
        average_extracted_data = average_extracted_columns.values.tolist()
        for row in average_extracted_data:
            if type(row[0]) is str:
                sheet.row += [row]
            else:
                continue
        for row in average_extracted_data:
            if type(row[0]) is int:
                sheet.row += [row]
            else:
                continue
        count = 1
        for i in range(1, sheet.number_of_rows()):
            sheet[i, 1] = round(sheet[i, 1], 6)  # 座標取小數點後6位
            sheet[i, 2] = round(sheet[i, 2], 6)  # 座標取小數點後6位
            sheet[i, 8] = 0  # State
            sheet[i, 9] = '區間測速'  # Source
            count += 1
        # fixed
        fixed_extracted_columns = fixed.iloc[:, [0, 1, 2, 3, 4, 5, 6]]
        fixed_extracted_data = fixed_extracted_columns.values.tolist()
        for row in fixed_extracted_data:
            sheet.row += [row]
        # combine
        combine_extracted_columns = combine.iloc[:, [0, 1, 2, 3, 4, 5, 6]]
        combine_extracted_data = combine_extracted_columns.values.tolist()
        for row in combine_extracted_data:
            sheet.row += [row]
        for i in range(count, sheet.number_of_rows()):  # 18 & 19改成1
            sheet[i, 1] = round(sheet[i, 1], 6)  # 座標取小數點後6位
            sheet[i, 2] = round(sheet[i, 2], 6)  # 座標取小數點後6位
            if sheet[i, 5] == 18 or sheet[i, 5] == 19:
                sheet[i, 5] = 1
            sheet[i, 7] = int(data_ver)  # Version
            sheet[i, 8] = 0  # State
            sheet[i, 9] = '固定式'  # Source
            count += 1
        # sea
        for i in range(1, sea.number_of_rows()):
            sheet.row += [sea.row[i][0:8]]
        for i in range(count, sheet.number_of_rows()):
            sheet[i, 1] = round(sheet[i, 1], 6)  # 座標取小數點後6位
            sheet[i, 2] = round(sheet[i, 2], 6)  # 座標取小數點後6位
            if sheet[i, 5] == 5 or sheet[i, 5] == 6:  # SEA 將 5 & 6 改成 1
                sheet[i, 5] = 1
            sheet[i, 8] = 0  # State
            sheet[i, 9] = 'SEA'  # Source
        r22_data.save_as(out_temp)
        repair_xls_format(out_temp, out_r22);
        delete_file(out_temp);
    else:
        print('data not found')
    print('end process r22')
########################################################################################################################


# R23 -> fixed+average+sea+tech+mobile (sea 5 & 6改成1)(tech 61 刪除, speed 填0)
def r23(fixed, average, sea, tech, mobile, six, combine):
    print('start process r23')
    
    r23_data = p.get_book(file_name=r23_path)
    out_r23 = os.path.join(out_path, 'Release', 'R23', f'V{build_R23}.{build_date}.01.23-ROW',
                           f'TW&SEA_{data_ver}_MioSpeedCam-newtype_v{build_R23}.xls')
    out_temp = os.path.join(out_path, 'Release', 'R23', f'V{build_R23}.{build_date}.01.23-ROW',
                           f'temp.xls')
    # 修改 'note' 工作表
    if 'note' in r23_data.sheet_names():
        sheet = r23_data['note']
        sheet[999, 5] = build_R23
    # 修改 'data' 工作表
    if 'data' in r23_data.sheet_names():
        sheet = r23_data['data']
        number_of_rows = sheet.number_of_rows()
        # 只保留第一列
        for i in range(number_of_rows - 1, 0, -1):
            del sheet.row[i]
        # average
        average_extracted_columns = average.iloc[:, [0, 1, 2, 3, 4, 5, 6, 11]]
        average_extracted_data = average_extracted_columns.values.tolist()

        # 加入 UID 去重邏輯
        seen_normal = set()
        seen_e = set()
        new_uid_base_normal = 8000  # 給一般數字用
        new_uid_base_e = 8000

        def get_new_uid(seen_set, is_e_type, uid_start):
            # 產生新的不重複 UID
            while str(uid_start) in seen_set:
                uid_start += 1
            new_uid = str(uid_start) + ('_E' if is_e_type else '')
            seen_set.add(str(uid_start))
            return new_uid, uid_start + 1

        for row in average_extracted_data:
            if type(row[0]) is str:
                sheet.row += [row]
            else:
                continue
        for row in average_extracted_data:
            if type(row[0]) is int:
                sheet.row += [row]
            else:
                continue
        count = 1
        for i in range(1, sheet.number_of_rows()):
            uid = str(sheet[i, 0])

            # 判斷是否為 _E 類型
            if uid.endswith('_E'):
                base = uid[:-2]
                if base in seen_e:
                    new_uid, new_uid_base_e = get_new_uid(seen_e, is_e_type=True, uid_start=new_uid_base_e)
                    sheet[i, 0] = new_uid
                else:
                    seen_e.add(base)
            else:
                if uid in seen_normal:
                    new_uid, new_uid_base_normal = get_new_uid(seen_normal, is_e_type=False, uid_start=new_uid_base_normal)
                    sheet[i, 0] = int(new_uid)
                else:
                    seen_normal.add(uid)
            sheet[i, 1] = round(sheet[i, 1], 6)  # 座標取小數點後6位
            sheet[i, 2] = round(sheet[i, 2], 6)  # 座標取小數點後6位
            sheet[i, 8] = 0  # state
            sheet[i, 9] = '區間測速'  # Source
            count += 1
        # fixed
        fixed_extracted_columns = fixed.iloc[:, [0, 1, 2, 3, 4, 5, 6]]
        fixed_extracted_data = fixed_extracted_columns.values.tolist()
        for row in fixed_extracted_data:
            sheet.row += [row]
        # combine
        combine_extracted_columns = combine.iloc[:, [0, 1, 2, 3, 4, 5, 6]]
        combine_extracted_data = combine_extracted_columns.values.tolist()
        for row in combine_extracted_data:
            sheet.row += [row]
        for i in range(count, sheet.number_of_rows()):
            sheet[i, 1] = round(sheet[i, 1], 6)  # 座標取小數點後6位
            sheet[i, 2] = round(sheet[i, 2], 6)  # 座標取小數點後6位
            sheet[i, 7] = int(data_ver)  # Version
            sheet[i, 8] = 0  # state
            sheet[i, 9] = '固定式'  # Source
            count += 1
        # sea
        for i in range(1, sea.number_of_rows()):
            sheet.row += [sea.row[i][0:8]]
        for i in range(count, sheet.number_of_rows()):
            sheet[i, 1] = round(sheet[i, 1], 6)  # 座標取小數點後6位
            sheet[i, 2] = round(sheet[i, 2], 6)  # 座標取小數點後6位
            if sheet[i, 5] == 5 or sheet[i, 5] == 6:  # SEA 將 5 & 6 改成 1
                sheet[i, 5] = 1
            sheet[i, 8] = 0  # state
            sheet[i, 9] = 'SEA'  # Source
            count += 1
        # tech
        tech_extracted_columns = tech.iloc[:, [0, 5, 6, 7, 8, 9, 10]]
        tech_extracted_data = tech_extracted_columns.values.tolist()
        for row in tech_extracted_data:
            if row[5] == 6:  # 只保留 6
                sheet.row += [row]
            else:
                continue
        for i in range(count, sheet.number_of_rows()):
            sheet[i, 1] = round(sheet[i, 1], 6)  # 座標取小數點後6位
            sheet[i, 2] = round(sheet[i, 2], 6)  # 座標取小數點後6位
            sheet[i, 4] = 0  # speed 填 0
            sheet[i, 7] = str(tech_ver)  # Version
            sheet[i, 8] = 0  # state
            sheet[i, 9] = '科技執法'  # Source
            count += 1
        # six
        six_extracted_columns = six.iloc[:, [0, 1, 2, 3, 4, 5, 6]]
        six_extracted_data = six_extracted_columns.values.tolist()
        for row in six_extracted_data:
            sheet.row += [row]
        for i in range(count, sheet.number_of_rows()):
            sheet[i, 1] = round(sheet[i, 1], 6)  # 座標取小數點後6位
            sheet[i, 2] = round(sheet[i, 2], 6)  # 座標取小數點後6位
            sheet[i, 4] = 0  # speed 填 0
            sheet[i, 7] = str(tech_ver)  # Version
            sheet[i, 8] = 0  # state
            sheet[i, 9] = '科技執法'  # Source
            count += 1
        # mobile
        mobile_extracted_columns = mobile.iloc[:, [0, 1, 2, 3, 4, 5, 6, 11]]
        mobile_extracted_data = mobile_extracted_columns.values.tolist()
        for row in mobile_extracted_data:
            sheet.row += [row]
        for i in range(count, sheet.number_of_rows()):
            sheet[i, 1] = round(sheet[i, 1], 6)  # 座標取小數點後6位
            sheet[i, 2] = round(sheet[i, 2], 6)  # 座標取小數點後6位
            sheet[i, 8] = 0  # state
            sheet[i, 9] = '移動式'  # Source
        r23_data.save_as(out_temp)
        repair_xls_format(out_temp, out_r23);
        delete_file(out_temp);
    else:
        print('data not found')
    print('end process r23')
########################################################################################################################


# R24 -> fixed+average+sea+tech+mobile (sea 5 & 6改成1)(增加 Bridge-type & sec)
def r24(fixed, average, sea, tech, mobile, combine):
    print('start process r24')
    
    r24_data = p.get_book(file_name=r24_path)
    out_r24 = os.path.join(out_path, 'Release', 'R24', f'V{build_R24}.{build_date}.01.24-ROW',
                           f'TW_{data_ver}_MioSpeedCam-9in1_v{build_R24}.xls')
    out_temp = os.path.join(out_path, 'Release', 'R24', f'V{build_R24}.{build_date}.01.24-ROW',
                           f'temp.xls')
    # 修改 'note' 工作表
    if 'note' in r24_data.sheet_names():
        sheet = r24_data['note']
        sheet[999, 5] = build_R24
    # 修改 'data' 工作表
    if 'data' in r24_data.sheet_names():
        sheet = r24_data['data']
        number_of_rows = sheet.number_of_rows()
        # 只保留第一列
        for i in range(number_of_rows - 1, 0, -1):
            del sheet.row[i]
        # average
        average_extracted_columns = average.iloc[:, [0, 1, 2, 3, 4, 5, 6, 7, 8, 11]]
        average_extracted_data = average_extracted_columns.values.tolist()
        for row in average_extracted_data:
            if type(row[0]) is str:
                sheet.row += [row]
            else:
                continue
        for row in average_extracted_data:
            if type(row[0]) is int:
                sheet.row += [row]
            else:
                continue
        count = 1
        for i in range(1, sheet.number_of_rows()):
            sheet[i, 1] = round(sheet[i, 1], 6)  # 座標取小數點後6位
            sheet[i, 2] = round(sheet[i, 2], 6)  # 座標取小數點後6位
            sheet[i, 10] = 0  # State
            sheet[i, 11] = '區間測速'  # Source
            count += 1
        # fixed
        fixed_extracted_columns = fixed.iloc[:, [0, 1, 2, 3, 4, 5, 6, 7, 8]]
        fixed_extracted_data = fixed_extracted_columns.values.tolist()
        for row in fixed_extracted_data:
            sheet.row += [row]
        # combine
        combine_extracted_columns = combine.iloc[:, [0, 1, 2, 3, 4, 5, 6, 7, 8]]
        combine_extracted_data = combine_extracted_columns.values.tolist()
        for row in combine_extracted_data:
            sheet.row += [row]
        for i in range(count, sheet.number_of_rows()):
            sheet[i, 1] = round(sheet[i, 1], 6)  # 座標取小數點後6位
            sheet[i, 2] = round(sheet[i, 2], 6)  # 座標取小數點後6位
            sheet[i, 9] = int(data_ver)  # Version
            sheet[i, 10] = 0  # State
            sheet[i, 11] = '固定式'  # Source
            count += 1
        # sea
        for i in range(1, sea.number_of_rows()):
            sheet.row += [sea.row[i][0:8]]
        for i in range(count, sheet.number_of_rows()):
            sheet[i, 1] = round(sheet[i, 1], 6)  # 座標取小數點後6位
            sheet[i, 2] = round(sheet[i, 2], 6)  # 座標取小數點後6位
            if sheet[i, 5] == 5 or sheet[i, 5] == 6:  # SEA 將 5 & 6 改成 1
                sheet[i, 5] = 1
            sheet[i, 9] = sheet[i, 7]  # Version
            sheet[i, 7] = 0  # bridge
            sheet[i, 8] = 0  # sec
            sheet[i, 10] = 0  # State
            sheet[i, 11] = 'SEA'  # Source
            count += 1
        # tech
        tech_extracted_columns = tech.iloc[:, [0, 5, 6, 7, 8, 9, 10, 11]]
        tech_extracted_data = tech_extracted_columns.values.tolist()
        for row in tech_extracted_data:
            sheet.row += [row]
        for i in range(count, sheet.number_of_rows()):
            sheet[i, 1] = round(sheet[i, 1], 6)  # 座標取小數點後6位
            sheet[i, 2] = round(sheet[i, 2], 6)  # 座標取小數點後6位
            sheet[i, 8] = 0  # sec
            sheet[i, 9] = str(tech_ver)  # Version
            sheet[i, 10] = 0  # State
            sheet[i, 11] = '科技執法'  # Source
            count += 1
        # mobile
        mobile_extracted_columns = mobile.iloc[:, [0, 1, 2, 3, 4, 5, 6, 7, 8, 11]]
        mobile_extracted_data = mobile_extracted_columns.values.tolist()
        for row in mobile_extracted_data:
            sheet.row += [row]
        for i in range(count, sheet.number_of_rows()):
            sheet[i, 1] = round(sheet[i, 1], 6)  # 座標取小數點後6位
            sheet[i, 2] = round(sheet[i, 2], 6)  # 座標取小數點後6位
            sheet[i, 10] = 0  # State
            sheet[i, 11] = '移動式'  # Source
        r24_data.save_as(out_temp)
        repair_xls_format(out_temp, out_r24);
        delete_file(out_temp);
    else:
        print('data not found')
    print('end process r24')
########################################################################################################################


# R41 -> (R12+average)(average只保留964, 964改成4)(是使用R12的data, 所以不能單獨使用)
def r41(average):
    print('start process r41')
    r12_path = os.path.join(out_path, 'Release', 'R12', f'LW_ABBY_AMY-V{build_R12}.{build_date}.01.12-ROW',
                            f'TW&SEA_{data_ver}_MioSpeedCam-newtype_v{build_R12}.xls')
    r12_data = p.get_book(file_name=r12_path)
    
    r41_data = p.get_book(file_name=r41_path)
    out_r41 = os.path.join(out_path, 'Release', 'R41', f'York-V{build_R41}_v{build_date_year}-ROW',
                           f'TW&SEA_{data_ver}_MioSpeedCam-newtype_v{build_R41}.xls')
    out_temp = os.path.join(out_path, 'Release', 'R41', f'York-V{build_R41}_v{build_date_year}-ROW',
                           f'temp.xls')
    r12_sheet = r12_data['data']
    # 修改 'note' 工作表
    if 'note' in r41_data.sheet_names():
        sheet = r41_data['note']
        sheet[999, 5] = build_R41
        # 修改 'data' 工作表
    if 'data' in r41_data.sheet_names():
        sheet = r41_data['data']
        number_of_rows = sheet.number_of_rows()
        # 只保留第一列
        for i in range(number_of_rows - 1, 0, -1):
            del sheet.row[i]
        count = 1
        # r12
        for i in range(1, r12_sheet.number_of_rows()):
            sheet[i, 1] = r12_sheet[i, 1]
            sheet[i, 2] = r12_sheet[i, 2]
            sheet[i, 3] = r12_sheet[i, 3]
            sheet[i, 4] = r12_sheet[i, 4]
            sheet[i, 5] = r12_sheet[i, 5]
            sheet[i, 6] = r12_sheet[i, 6]
            sheet[i, 7] = r12_sheet[i, 7]
            count += 1
        # average
        average_extracted_columns = average.iloc[:, [0, 1, 2, 3, 4, 5, 6, 11]]
        average_extracted_data = average_extracted_columns.values.tolist()
        for row in average_extracted_data:
            if row[5] == 964:  # 只保留 964
                sheet.row += [row]
            else:
                continue
        for i in range(count, sheet.number_of_rows()):
            sheet[i, 0] = ''  # UID 清空
            sheet[i, 1] = round(sheet[i, 1], 6)  # 座標取小數點後6位
            sheet[i, 2] = round(sheet[i, 2], 6)  # 座標取小數點後6位
            sheet[i, 5] = 4  # 964 中的camera type 改成 4
            count += 1
        r41_data.save_as(out_temp)
        repair_xls_format(out_temp, out_r41);
        delete_file(out_temp);
    else:
        print('data not found')
    print('end process r41')
########################################################################################################################


def r99(fixed, average, tech, mobile, motorcycle, popular, svd, combine):
    print('start process r99')
    
    r99_data = p.get_book(file_name=r99_path)
    out_r99 = os.path.join(out_path, 'Release', 'R99', f'V{build_R99}.{build_date}.01.99-ROW',
                           f'TW_{data_ver}_MioSpeedCam-99_v{build_R99}.xls')
    out_temp = os.path.join(out_path, 'Release', 'R99', f'V{build_R99}.{build_date}.01.99-ROW',
                           f'temp.xls')
    # 修改 'note' 工作表
    if 'note' in r99_data.sheet_names():
        sheet = r99_data['note']
        sheet[999, 5] = build_R99
    # 修改 'data' 工作表
    if 'data' in r99_data.sheet_names():
        sheet = r99_data['data']
        number_of_rows = sheet.number_of_rows()
        # 只保留第一列
        for i in range(number_of_rows - 1, 0, -1):
            del sheet.row[i]
        # average and svd
        average_extracted_columns = average.iloc[:, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 11]]
        average_extracted_data = average_extracted_columns.values.tolist()
        svd_extracted_columns = svd.iloc[:, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14]]
        svd_extracted_data = svd_extracted_columns.values.tolist()
        for row in average_extracted_data:
            if type(row[0]) is str:
                sheet.row += [row]
            else:
                continue
        for row in svd_extracted_data:
            if type(row[0]) is str:
                sheet.row += [row]
            else:
                continue
        for row in average_extracted_data:
            if type(row[0]) is int:
                sheet.row += [row]
            else:
                continue
        for row in svd_extracted_data:
            if type(row[0]) is int:
                sheet.row += [row]
            else:
                continue
        count = 1
        for i in range(1, sheet.number_of_rows()):
            sheet[i, 1] = round(sheet[i, 1], 6)  # 座標取小數點後6位
            sheet[i, 2] = round(sheet[i, 2], 6)  # 座標取小數點後6位
            sheet[i, 9] = str(sheet[i, 9]).replace(' ', '')  # 去除空格
            sheet[i, 11] = str(sheet[i, 11])
            sheet[i, 13] = 0  # State
            if sheet[i, 14] == '':
                sheet[i, 14] = '區間測速'  # Source
            count += 1
        # fixed
        fixed_extracted_columns = fixed.iloc[:, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 15]]
        fixed_extracted_data = fixed_extracted_columns.values.tolist()
        for row in fixed_extracted_data:
            sheet.row += [row]
        # combine
        combine_extracted_columns = combine.iloc[:, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 15]]
        combine_extracted_data = combine_extracted_columns.values.tolist()
        for row in combine_extracted_data:
            sheet.row += [row]
        for i in range(count, sheet.number_of_rows()):
            sheet[i, 1] = round(sheet[i, 1], 6)  # 座標取小數點後6位
            sheet[i, 2] = round(sheet[i, 2], 6)  # 座標取小數點後6位
            sheet[i, 8] = 0  # sec
            sheet[i, 9] = str(sheet[i, 9]).replace(' ', '')  # 去除空格
            sheet[i, 11] = str(sheet[i, 11])
            sheet[i, 12] = int(data_ver)  # Version
            sheet[i, 13] = 0  # State
            sheet[i, 14] = '固定式'  # Source
            count += 1
        # tech
        tech_extracted_columns = tech.iloc[:, [0, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 4]]
        tech_extracted_data = tech_extracted_columns.values.tolist()
        for row in tech_extracted_data:
            sheet.row += [row]
        for i in range(count, sheet.number_of_rows()):
            sheet[i, 1] = round(sheet[i, 1], 6)  # 座標取小數點後6位
            sheet[i, 2] = round(sheet[i, 2], 6)  # 座標取小數點後6位
            sheet[i, 8] = 0  # sec
            sheet[i, 9] = str(sheet[i, 9]).replace(' ', '')  # 去除空格
            sheet[i, 11] = str(sheet[i, 11])
            sheet[i, 12] = str(tech_ver)  # Version
            sheet[i, 13] = 0  # State
            sheet[i, 14] = '科技執法'  # Source
            count += 1
        # mobile
        mobile_extracted_columns = mobile.iloc[:, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 15, 11]]
        mobile_extracted_data = mobile_extracted_columns.values.tolist()
        for row in mobile_extracted_data:
            sheet.row += [row]
        for i in range(count, sheet.number_of_rows()):
            sheet[i, 1] = round(sheet[i, 1], 6)  # 座標取小數點後6位
            sheet[i, 2] = round(sheet[i, 2], 6)  # 座標取小數點後6位
            sheet[i, 8] = 0  # sec
            sheet[i, 9] = str(sheet[i, 9]).replace(' ', '')  # 去除空格
            sheet[i, 11] = str(sheet[i, 11])
            sheet[i, 13] = 0  # State
            sheet[i, 14] = '移動式'  # Source
            count += 1
        # motorcycle
        motorcycle_extracted_columns = motorcycle.iloc[:, [0, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 2]]
        motorcycle_extracted_data = motorcycle_extracted_columns.values.tolist()
        for row in motorcycle_extracted_data:
            sheet.row += [row]
        for i in range(count, sheet.number_of_rows()):
            sheet[i, 1] = round(sheet[i, 1], 6)  # 座標取小數點後6位
            sheet[i, 2] = round(sheet[i, 2], 6)  # 座標取小數點後6位
            sheet[i, 8] = 0  # sec
            sheet[i, 9] = str(sheet[i, 9]).replace(' ', '')  # 去除空格
            sheet[i, 11] = str(sheet[i, 11])
            sheet[i, 12] = str(motorcycle_ver)  # Version
            sheet[i, 13] = 0  # State
            sheet[i, 14] = '機車'  # Source
            count += 1
        # popular
        popular_extracted_columns = popular.iloc[:, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 15, 11]]
        popular_extracted_data = popular_extracted_columns.values.tolist()
        for row in popular_extracted_data:
            sheet.row += [row]
        for i in range(count, sheet.number_of_rows()):
            sheet[i, 1] = round(sheet[i, 1], 6)  # 座標取小數點後6位
            sheet[i, 2] = round(sheet[i, 2], 6)  # 座標取小數點後6位
            sheet[i, 8] = 0  # sec
            sheet[i, 9] = str(sheet[i, 9]).replace(' ', '')  # 去除空格
            sheet[i, 11] = str(sheet[i, 11])
            sheet[i, 12] = str(sheet[i, 12])  # Version
            sheet[i, 13] = 0  # State
            sheet[i, 14] = '常事故點'  # Source

        r99_data.save_as(out_temp)
        repair_xls_format(out_temp, out_r99);
        delete_file(out_temp);
    else:
        print('data not found')
    print('end process r99')
########################################################################################################################


# source data path
SuorceData_path = r'C:\TWSC_Toolset\TWN\SourceData'
# average
average_path = (os.path.join(SuorceData_path, data_ver, f'average_{data_ver}.xlsx'))
average_data = pd.read_excel(average_path, sheet_name='Taiwan區間測速')

# fixed
fixed_path = (os.path.join(SuorceData_path, data_ver, f'fixed_{data_ver}.xlsx'))
fixed_data = pd.read_excel(fixed_path, sheet_name='Taiwan固定式測速')

# combine
combine_path = (os.path.join(SuorceData_path, data_ver, f'combine_{data_ver}.xlsx'))
combine_data = pd.read_excel(combine_path, sheet_name='固定式合併科技執法')
# print(combine_data)

# mobile
mobile_path = (os.path.join(SuorceData_path, data_ver, f'mobile_{data_ver}.xlsx'))
mobile_data = pd.read_excel(mobile_path, sheet_name='Taiwan移動式')

# tech
tech_path = (os.path.join(SuorceData_path, data_ver, f'tech_{tech_ver}.xlsx'))
tech_data = pd.read_excel(tech_path, sheet_name='Taiwan科技執法')

# motorcycle
motorcycle_path = (os.path.join(SuorceData_path, data_ver, f'motorcycle_{data_ver}.xlsx'))
motorcycle_data = pd.read_excel(motorcycle_path, sheet_name='機車')

# popular
popular_path = (os.path.join(SuorceData_path, data_ver, f'popular_{data_ver}.xlsx'))
popular_data = pd.read_excel(popular_path, sheet_name='Taiwan常事故點')

# 6in1
six_in_one_path = (os.path.join(SuorceData_path, data_ver, f'6in1_{data_ver}.xlsx'))
six_in_one_data = pd.read_excel(six_in_one_path, sheet_name='6合1')

# SVD
svd_path = (os.path.join(SuorceData_path, data_ver, f'SVD_{data_ver}.xlsx'))
svd_data = pd.read_excel(svd_path, sheet_name='SVD')

# new SEA
new_SEA_path = (os.path.join(SuorceData_path, data_ver, f'SEA_allothers_{SEA_ver}.xls'))
new_SEA = p.get_book(file_name=new_SEA_path)
new_SEA_data = new_SEA['data']

# old SEA
old_SEA_path = (os.path.join(SuorceData_path, 'SEA_20230217', f'SEA_allothers_20230217.xls'))
old_SEA = p.get_book(file_name=old_SEA_path)
old_SEA_data = old_SEA['data']
########################################################################################################################

make_dir()
r01_388(fixed_data, combine_data)
r01_only_lw(fixed_data, combine_data)
r28_r52_r60r62_r58()  # (是使用r01_only_lw 的 data, 所以不能單獨使用)
r01_tw_sea_lw(fixed_data, combine_data, old_SEA_data)
r12(fixed_data, combine_data, new_SEA_data)
r21(fixed_data, combine_data, average_data)
r22(fixed_data, combine_data, average_data, old_SEA_data)
r23(fixed_data, average_data, new_SEA_data, tech_data, mobile_data, six_in_one_data, combine_data)
r24(fixed_data, average_data, new_SEA_data, tech_data, mobile_data, combine_data)
r41(average_data)  # (是使用R12的data, 所以不能單獨使用)
r99(fixed_data, average_data, tech_data, mobile_data, motorcycle_data, popular_data, svd_data, combine_data)

