import os
import subprocess
import platform
import requests
from colorama import Fore, Style, init
import subprocess
init(autoreset=True)

VERSION = "1.0.0"
VERSION_URL = "https://raw.githubusercontent.com/VexlsGG/VexTweaks/main/version.txt"

from colorama import Fore, Style, init
init(autoreset=True)

def print_header():
    rainbow = [Fore.RED, Fore.YELLOW, Fore.GREEN, Fore.CYAN, Fore.BLUE, Fore.MAGENTA]
    logo = [
        "██╗   ██╗███████╗██╗  ██╗    ████████╗██╗    ██╗███████╗ █████╗ ██╗  ██╗███████╗",
        "██║   ██║██╔════╝╚██╗██╔╝    ╚══██╔══╝██║    ██║██╔════╝██╔══██╗██║ ██╔╝██╔════╝",
        "██║   ██║█████╗   ╚███╔╝        ██║   ██║ █╗ ██║█████╗  ███████║█████╔╝ ███████╗",
        "╚██╗ ██╔╝██╔══╝   ██╔██╗        ██║   ██║███╗██║██╔══╝  ██╔══██║██╔═██╗ ╚════██║",
        " ╚████╔╝ ███████╗██╔╝ ██╗       ██║   ╚███╔███╔╝███████╗██║  ██║██║  ██╗███████║",
        "  ╚═══╝  ╚══════╝╚═╝  ╚═╝       ╚═╝    ╚══╝╚══╝ ╚══════╝╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝"
    ]
    for i, line in enumerate(logo):
        print(rainbow[i % len(rainbow)] + line)

    print(Fore.CYAN + "REPO: https://github.com/VexlsGG/VexTweaks")
    print(Fore.YELLOW + f"CURRENT VERSION: v{VERSION}")
    print("=" * 80)

def specs():
    print(Fore.LIGHTBLUE_EX + f"System: {platform.system()} {platform.release()}")
    print(f"CPU: {get_cpu_name()}")
    print(f"GPU: {get_gpu_name()}")
    print("=" * 80 + Style.RESET_ALL)

import subprocess

def get_cpu_name():
    try:
        powershell_path = r"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe"
        result = subprocess.run(
            [powershell_path, "-Command", "Get-CimInstance -ClassName Win32_Processor | Select-Object -ExpandProperty Name"],
            capture_output=True,
            text=True,
            timeout=3
        )
        cpu_name = result.stdout.strip()
        return cpu_name if cpu_name else "Unknown CPU"
    except Exception as e:
        return f"Error: {e}"

def get_gpu_name():
    try:
        powershell_path = r"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe"
        result = subprocess.run(
            [powershell_path, "-Command", "Get-CimInstance Win32_VideoController | Select-Object -ExpandProperty Name"],
            capture_output=True,
            text=True,
            timeout=3
        )
        gpu_name = result.stdout.strip()
        if gpu_name:
            return gpu_name
    except:
        pass
    return "Unknown GPU"



def check_for_updates():
    try:
        response = requests.get(VERSION_URL, timeout=5)
        if response.status_code != 200:
            print(Fore.RED + f"⚠️ GitHub returned: {response.status_code} - {response.reason}")
            return
        latest_version = response.text.strip()
        if latest_version != VERSION:
            print(Fore.GREEN + f"🔄 A new version is available: v{latest_version}!")
            print("Visit GitHub to download the latest release.")
        else:
            print(Fore.GREEN + "✅ You are using the latest version.")
    except:
        print(Fore.RED + "⚠️ Could not check for updates. Is GitHub reachable?")

def menu():
    while True:
        os.system('cls' if os.name == 'nt' else 'clear')
        print_header()
        specs()
        print(Fore.GREEN + """
[A] Apply All Tweaks         [B] Create Restore Point     [C] Backup Registry
[D] Restore Registry         [E] Clear Temp + Prefetch    [F] Refresh Network Stack
[G] Launch ISLC              [H] Launch Fortnite Clean    [I] Open Device Cleanup
[J] Check for Updates        [X] Exit
""")
        choice = input(Fore.CYAN + "Select option: ").strip().upper()

        if choice == "A":
            subprocess.run(["sc", "stop", "XblGameSave"], stdout=subprocess.DEVNULL)
            subprocess.run(["sc", "config", "XblGameSave", "start=", "disabled"], stdout=subprocess.DEVNULL)
            subprocess.run(["sc", "stop", "DiagTrack"], stdout=subprocess.DEVNULL)
            subprocess.run(["sc", "config", "DiagTrack", "start=", "disabled"], stdout=subprocess.DEVNULL)
            print("✅ All tweaks applied.")
        
        elif choice == "B":
            os.system('powershell -Command "Checkpoint-Computer -Description \\"VexTweaks Boost\\" -RestorePointType \\"MODIFY_SETTINGS\\""')
            print("✅ Restore point created.")

        elif choice == "C":
            os.makedirs("backup", exist_ok=True)
            os.system(r'reg export HKCU backup\HKCU_Backup.reg /y')
            os.system(r'reg export "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile" backup\SystemProfile_Backup.reg /y')
            print("✅ Registry backup saved.")

        elif choice == "D":
            os.system(r'reg import backup\HKCU_Backup.reg')
            os.system(r'reg import backup\SystemProfile_Backup.reg')
            os.system(r'reg import backup\TCP_Backup.reg')
            print("✅ Registry restored.")

        elif choice == "E":
            os.system(r'del /f /s /q %temp%\*')
            os.system(r'rd /s /q %temp%')
            os.system(r'md %temp%')
            os.system("del /f /s /q C:\\Windows\\Prefetch\\*")
            print("✅ Temp and Prefetch cleaned.")

        elif choice == "F":
            os.system("ipconfig /flushdns")
            os.system("ipconfig /release")
            os.system("ipconfig /renew")
            os.system("netsh winsock reset")
            os.system("netsh int ip reset")
            print("✅ Network stack refreshed.")

        elif choice == "G":
            os.system("start ISLC/ISLC.exe")

        elif choice == "H":
            os.system("taskkill /f /im GameBar.exe")
            os.system("taskkill /f /im XboxAppServices.exe")
            os.system('start "" /high "com.epicgames.launcher://apps/Fortnite?action=launch&silent=true"')

        elif choice == "I":
            os.system("set devmgr_show_nonpresent_devices=1 && start devmgmt.msc")

        elif choice == "J":
            check_for_updates()

        elif choice == "X":
            break

        else:
            print("Invalid input.")

        input(Fore.LIGHTBLACK_EX + "\nPress Enter to return to the menu...")

menu()
