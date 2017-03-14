#!/usr/bin/python

import os
import sys

if(len(sys.argv) == 4):
    db_admin = sys.argv[1]
    new_role_name = sys.argv[2]
    new_db_name = sys.argv[3]

    os.environ["PGUSER"] = db_admin

    print("Create new role: " + new_role_name) 
    status = os.system("createuser -P -s -d -r " + new_role_name)
    if(status != 0):
        exit(1)

    os.environ["PGUSER"] = new_role_name
    print("Create new database: " + new_db_name)
    status = os.system("createdb " + new_db_name)
    if(status != 0):
        exit(1)
else:
    print("Erro: Not enough arguments")
    print("py db_setup.py db_admin new_role_name new_db_name")