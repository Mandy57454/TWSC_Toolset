File structures:

\
|-QtCore4.dll                                   --- Qt library for SCConverter_gui.exe
|-QtGui4.dll                                    --- Qt library for SCConverter_gui.exe
|-SCConverter_cmd.exe                           --- command line tool
|-SCConverter_gui.exe                           --- GUI tool
|-SCConverter_MiVue.exe                         --- command line tool for MiVue
|-sample_data
    |-Rus_speed_Cam_22012013_new_GPSCLUB.xls    --- Russia speed cam data (excel format)
    |-TW_SC.csv                                 --- Taiwan speed cam data (csv format)
    |-TW_SC.xls                                 --- Taiwan speed cam data (excel format)
    |-TW_SC_sim_large_amout.csv                 --- Taiwan test speed cam data (not real speed cam) (csv format)
    
    
Usages:
(a) SCConverter_cmd.exe:
 SCConverter_cmd.exe 
  path_to_input_file (.xls .csv)
  path_to_output_file
  [-f (fast reading mode for .xls)]
  [-n (save speed camera name if exists)]
  [-l (output to log file)]
  
 e.g. SCConverter_cmd.exe TW_SC.csv TW_SC.sc -n -l convert.log
      SCConverter_cmd.exe TW_SC.xls TW_SC.sc -f -n -l convert.log
      
(b) SCConverter_gui.exe:
  1. run SCConverter_gui.exe
  2. choose source file path
  3. choose output file path
  4. Tap "Convert"
  
(c) SCConverter_MiVue.exe:
  This command line tool is developed for MiVue Converter
  1. Put SCConverter_MiVue.exe into MivueConverter\Converter988
  2. Edit MivueConverter\Setting.ini, add a model for SCConverter
    e.g.
    [ModelInfo]
    ModelCount = 2
    
    [Model_1]
    (ignored...)
    
    [Model_2]
    DeviceName = MiVue 988  
    ConvertExe = \Converter988\SCConverter_MiVue.exe
    SuccessLogFile = \Converter988\SuccessLogFile.txt   
    ErrorLogFile = \Converter988\ErrorLogFile.txt  
    RawFile = \Converter988\speed.sc



Usage:
        SCConverter_MiVue.exe
         path_to_input_file (.xls .csv)
        path_to_output_file
        [-f (fast reading mode for .xls)]
        [-l (output to log file)]


SCConverter_MiVue.exe Z:/DVR/EU/20160504_v35EU+Georgia/638/eu_20160503_MioSpeedCam.csv Z:/DVR/EU/20160504_v35EU+Georgia/new/Speedcam_Data_EU.bin -l

SCConverter_MiVue.exe Z:/DVR/Russia/20160526_v90/6series/ROU_20160526_MioSpeedCam.csv Z:/DVR/Russia/20160526_v90/6series/Speedcam_Data_RUS.bin -l