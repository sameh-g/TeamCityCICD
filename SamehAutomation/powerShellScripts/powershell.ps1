   #$solutionSource  = "D:\Biohome_GIT\src"
   #$publishProfile = "Dev"
   #$configuration="Debug"
   #$build_rootFolder= "D:\DWS_Packages\Sprint17\Build#1.40-09112017\UIServer"
   #test commit
    param 
    ( 
        [Parameter(Mandatory=$true)][String] $solutionSource, #solution folder path
        [Parameter(Mandatory=$true)][String] $build_rootFolder, #folder to put published files in
        [Parameter(Mandatory=$true)][String] $configuration,#Publish mode ex. Debug,DEV,Test
        [Parameter(Mandatory=$true)][String] $publishProfile #Publish profile ex. Debug,DEV,Test
    )   
    $RootPath = Split-Path $PSScriptRoot -Parent #Get full path of the folder that contains scripts
    $publishWepAppScriptPath = $RootPath + '\Common\Publish_WebApp.ps1' #publish Web App script path

    $published_folderDirecotry="C:\inetpub\wwwroot\"

    #Demand Plan API Project Constants
    $csprojFileName_DemandPlanApi = "\Syngenta.DWS.API.DemandPlan.csproj"
    $DemandPlanApiProjectName="Syngenta.DWS.API.DemandPlan"
    $DemandPlanApiPublishedFolderName = "Syngenta.DWS.Web.API.DemandPlan"


    #Publish Demand Plan API
    Invoke-Expression "& `"$publishWepAppScriptPath`" `"$solutionSource`" `"$build_rootFolder`" `"$publishProfile`" `"$configuration`" `"$published_folderDirecotry`" `"$DemandPlanApiProjectName`" `"$csprojFileName_DemandPlanApi`" `"$DemandPlanApiPublishedFolderName`" "
