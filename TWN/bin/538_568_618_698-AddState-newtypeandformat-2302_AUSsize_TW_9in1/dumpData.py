import struct
import os
import sys
import numpy as np

# typedef struct _SPEEDCAMGLBHEADER {
#     short usST_Lon;
#     short usST_Lat;
#     short usED_Lon;
#     short usED_Lat;
#     short SB_NUM_Lon;
#     short SB_NUM_Lat;
#     short LB_NUM_Lon;
#     short LB_NUM_Lat;
#     unsigned char ubVersion;
#     unsigned char Dummy[3];
# } SPEEDCAMGLBHEADER;

# typedef struct _SPEEDCAMGLBHEADER_EX {
#     int ulDBVersion
#     unsigned char ubTransFormDate[DBVERSIONLENGTH]
# } SPEEDCAMGLBHEADER_EX

SPEEDCAMGLBHEADER_fmt = '<hhhhhhhhB3s'
SPEEDCAMGLBHEADER_EX_fmt = '<i12s'
SPEEDCAMERALAYER_fmt = '<hhhhh'

def read_header(f):
    buff = f.read(struct.calcsize(SPEEDCAMGLBHEADER_fmt))
    usST_Lon, usST_LAT, usED_Lon, usED_Lat, SB_NUM_lon, SB_NUM_lat, LB_NUM_lon, LB_NUM_lat, ubVersion, dummy = struct.unpack_from(SPEEDCAMGLBHEADER_fmt,
                                                                                                                                                   buff, 0)
    header = {'usST_Lon': usST_Lon, 'usST_LAT': usST_LAT, 'usED_Lon': usED_Lon, 'usED_Lat': usED_Lat, 'SB_NUM_lon': SB_NUM_lon,
              'SB_NUM_lat': SB_NUM_lat, 'LB_NUM_lon': LB_NUM_lon, 'LB_NUM_lat': LB_NUM_lat, 'ubVersion': ubVersion, 'dummy': dummy}

    if ubVersion > 1:
        buff = f.read(struct.calcsize(SPEEDCAMGLBHEADER_EX_fmt))
        ulDBVersion, ubTransFormDate = struct.unpack_from(
            SPEEDCAMGLBHEADER_EX_fmt, buff, 0)
    else:
        ulDBVersion = 0
        ubTransFormDate = ''
    header_ex = {'ulDBVersion': ulDBVersion,
                 'ubTransFormDate': ubTransFormDate}

    if LB_NUM_lon * LB_NUM_lat < 500:
        layout_array_fmt = '<{}I'.format(LB_NUM_lat*LB_NUM_lon)
        buff = f.read(struct.calcsize(layout_array_fmt))
        layout_addr = np.array(struct.unpack_from(layout_array_fmt, buff, 0))
    else:
        layout_addr = []

    return header, header_ex, layout_addr

# typedef struct _SPEEDCAMERALAYER {
#     short TotalPoints;
#     short ST_Lat;
#     short ST_Lon;
#     short ED_lat;
#     short ED_lon;

# } SPEEDCAMERALAYER;

def read_layer(f, offset):
    f.seek(offset)
    buff = f.read(struct.calcsize(SPEEDCAMERALAYER_fmt))
    total, ST_Lat, ST_Lon, ED_Lat, ED_Lon = struct.unpack_from(SPEEDCAMERALAYER_fmt, buff, 0)
    layout = {'total': total, 'ST_Lat': ST_Lat,
              'ST_Lon': ST_Lon, 'ED_Lat': ED_Lat, 'ED_Lon': ED_Lon}
    return layout

# typedef struct _SPEEDCAMERALAYER {
#     short TotalPoints;
#     short ST_Lat;
#     short ST_Lon;
#     short ED_lat;
#     short ED_lon;

# } SPEEDCAMERALAYER;

def read_small_layer_table(f, large_offset, small_layer_count):
    layout_array_fmt = '<{}I'.format(small_layer_count)
    f.seek(large_offset + struct.calcsize(SPEEDCAMERALAYER_fmt))
    buff = f.read(struct.calcsize(layout_array_fmt))
    layout_addr = np.array(struct.unpack_from(layout_array_fmt, buff, 0))
    return layout_addr

def print_usage():
    print('usage: \n\tdump\n\t\tpython dumpData.py -f=\'filename.bin\'\n\t\toptions\n\t\t  -s: dump small layer detail\n\tzip data\n\t\tpython dumpData.py -f=\'inputdata.bin\' -c\'outdata.bin\'')



def convert_bin(src, dst):
    print('convert_bin', src, dst)
    file_size = os.path.getsize(src)

    f_src = open(src, 'rb')
    f_dst = open(dst, 'wb')
    header, header_ex, layout_addr = read_header(f_src)
    f_dst.write(struct.pack(SPEEDCAMGLBHEADER_fmt,header['usST_Lon'], header['usST_LAT'], header['usED_Lon'], header['usED_Lat'],
        header['SB_NUM_lon'], header['SB_NUM_lat'], header['LB_NUM_lon'], header['LB_NUM_lat'],header['ubVersion'], header['dummy']))
    
    if header['ubVersion'] > 1:
        f_dst.write(struct.pack(SPEEDCAMGLBHEADER_EX_fmt, header_ex['ulDBVersion'], header_ex['ubTransFormDate']))
    
    layout_begin_addr = 0
    if len(layout_addr) > 1:
        layout_begin_addr = f_dst.tell()
        f_dst.write(struct.pack('<{}I'.format(len(layout_addr)), *layout_addr))

    new_layout_addr = []

    cit = np.nditer(layout_addr, flags=["c_index"])
    totalPOI = 0
    total0Layer = 0
    total_extra_size = 0
    while not cit.finished:
        next_addr = 0
        if (cit.index + 1) == np.size(layout_addr):
            next_addr = file_size
        else:
            next_addr = layout_addr[cit.index+1]
        layout = read_layer(f_src, cit[0])
        totalPOI += layout['total']

        f_dst.seek(0, 2)

        new_layout_addr.append(f_dst.tell())
        f_dst.write(struct.pack(SPEEDCAMERALAYER_fmt, layout['total'], layout['ST_Lat'], layout['ST_Lon'], layout['ED_Lat'], layout['ED_Lon']))

        if layout['total'] > 0:
            small_layer_addr = read_small_layer_table(f_src, cit[0], header['SB_NUM_lon'] * header['SB_NUM_lat'])
            
            # move to end of file
            f_dst.seek(0, 2)
            small_layer_begin = f_dst.tell()
            f_dst.write(struct.pack('<{}I'.format(len(small_layer_addr)), *small_layer_addr))
            new_small_layout_addr = []
            sl_cit = np.nditer(small_layer_addr, flags=["c_index"])
            while not sl_cit.finished:
                new_small_layout_addr.append(f_dst.tell())
                if (sl_cit.index + 1) == np.size(small_layer_addr):
                    last_small_addr = next_addr
                else:
                    last_small_addr = small_layer_addr[sl_cit.index + 1]
                small_layer_trunk = f_src.read(last_small_addr - sl_cit[0])
                f_dst.write(small_layer_trunk)
                sl_cit.iternext()
            if len(new_small_layout_addr) != len(small_layer_addr):
                print('wrong while handle small layer')
                return
            f_dst.seek(small_layer_begin)
            f_dst.write(struct.pack('<{}I'.format(len(new_small_layout_addr)), *new_small_layout_addr))
        cit.iternext()
    f_dst.seek(layout_begin_addr)
    f_dst.write(struct.pack('<{}I'.format(len(new_layout_addr)), *new_layout_addr))

    f_src.close()
    f_dst.close()


def read_bin(argv):
    if len(argv) == 1:
        print_usage()
        return
    bin_file = ''
    out_file = ''
    dump_small_layer = False
    zip_file = False
    for index, arg in enumerate(argv):
        if arg.find('-f=') == 0 and len(arg.split('=',1)) == 2:
            bin_file = arg.split('=')[1]
        if arg == '-s':
            dump_small_layer = True
        if arg.find('-c=') == 0 and len(arg.split('=',1)) == 2:
            zip_file = True
            out_file = arg.split('=')[1]
    if bin_file == '':
        print_usage()
        return
    if zip_file and out_file == '':
        print_usage()
        return
    if zip_file:
        convert_bin(bin_file, out_file)
        return

    print('arg', argv, bin_file, dump_small_layer)
    #bin_file='Speedcam_Data_AUS.bin'
    file_size = os.path.getsize(bin_file)

    f = open(bin_file, 'rb')
    header, header_ex, layout_addr = read_header(f)
    print('\n ---- dump {} ----- '.format(bin_file))
    print('\n----------  Header -------------')
    print('version: {}, dummy {}'.format(header['ubVersion'], header['dummy']))
    print('ST_lon: {}, ST_Lat: {}, ED_Lon: {}, ED_Lat: {}'.format(
        header['usST_Lon'], header['usST_LAT'], header['usED_Lon'], header['usED_Lat']))
    print('SB_NUM_lon: {}, SB_NUM_lat: {}, LB_NUM_lon: {}, LB_NUM_lat: {}'.format(
        header['SB_NUM_lon'], header['SB_NUM_lat'], header['LB_NUM_lon'], header['LB_NUM_lat']))
    if header['ubVersion'] > 1:
        print('\n----------  Header Ex -------------')
        print('Ex version: {}, date: {}'.format(
            header_ex['ulDBVersion'], header_ex['ubTransFormDate']))
    if header['LB_NUM_lon'] * header['LB_NUM_lat'] < 500:
        print('\n----------  Large layout -------------')
        print('layout size: {}'.format(np.size(layout_addr)))
        print('layout address: {}'.format(layout_addr))
    print('\ncurrent file position is {}\n'.format(f.tell()))

    cit = np.nditer(layout_addr, flags=["c_index"])
    totalPOI = 0
    total0Layer = 0
    total_extra_size = 0
    while not cit.finished:
        next_addr = 0
        if (cit.index + 1) == np.size(layout_addr):
            next_addr = file_size
        else:
            next_addr = layout_addr[cit.index+1]
        print('-- dump layout {}, begin offset: {}, size: {}'.format(cit.index,
                  cit[0], next_addr - cit[0]))
        layout = read_layer(f, cit[0])
        totalPOI += layout['total']
        if layout['total'] > 0:
            print('   ', layout)
            small_layer_addr = read_small_layer_table(f, cit[0], header['SB_NUM_lon'] * header['SB_NUM_lat'])
            #print('   --- dump small layer address: {}'.format(small_layer_addr))
            sl_cit = np.nditer(small_layer_addr, flags=["c_index"])
            smallPOIs = 0
            totalSmallLayer = 0
            total_extra_sm_size = 0
            if dump_small_layer:
                print('      dump small layer')
            while not sl_cit.finished:
                small_layout = read_layer(f, sl_cit[0])
                if (sl_cit.index + 1) == np.size(small_layer_addr):
                    last_small_addr = next_addr
                else:
                    last_small_addr = small_layer_addr[sl_cit.index + 1]
                totalSmallLayer += small_layout['total']
                # print('   --- dump small layout {}, begin offset: {}, size: {}'.format(sl_cit.index,
                #   sl_cit[0], last_small_addr - sl_cit[0]))
                if small_layout['total'] > 0:
                    if dump_small_layer:
                        print('      ', small_layout)
                    smallPOIs += small_layout['total']
                    total_extra_sm_size += last_small_addr - sl_cit[0] - 10
                else:
                    totalSmallLayer += 1
                sl_cit.iternext()
            total_extra_size += total_extra_sm_size
            print('-- end dump layout, total {}, extra size is {}, {} layers have no POI data'.format(smallPOIs, total_extra_sm_size, total0Layer))
        else:
            total0Layer += 1
        # print('  total: %d, ST [%d, %d], ED [%d, %d]'.format(
        #     layout['total'], layout['ST_Lat'], layout['ST_Lon'], layout['ED_Lat'], layout['ED_Lon']))
        cit.iternext()
    print('   -- end dump layout, total {}, {} layers have no POI data'.format(totalPOI, totalSmallLayer))
    print('\nend dump, size is {}, total extra size is {}\n'.format(file_size, total_extra_size))
    print('----------------------------\n')
    f.close()



if __name__ == '__main__':
    read_bin(sys.argv)

#read_bin('Speedcam_Data_AUS0.bin')
#read_bin('Speedcam_Data_AUS1.bin')
#read_bin('Speedcam_Data_FEU.bin')
