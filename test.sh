#! /bin/bash

mono bin/Debug/duo2c testfiles/list.mod testfiles/link.mod -o testfiles/bin/link -e Link -K testfiles/ir
testfiles/bin/link
