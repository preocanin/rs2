(function () {

    angular
        .module('app')
        .controller('dataCtrl', dataCtrl);

    dataCtrl.$inject = ['RecordService', '$location', '$rootScope', 'toastr', '$scope', '$http'];

    function dataCtrl(RecordService, $location, $rootScope, toastr, $scope, $http) {
        "use strict";

        var vm = this;
        vm.unos = false;
        vm.prikaz = true;
        vm.izmena = false;
        vm.podatak = {};
        vm.records = [];

        var columnDefs = [
            { headerName: "Pre X", field: "bx" },
            { headerName: "Pre Y", field: "by" },
            { headerName: "Posle X", field: "ax" },
            { headerName: "Posle Y", field: "ay" },
        ];

        vm.gridOptions = {
            enableColResize: true,
            debug: true,
            rowSelection: 'multiple',
            rowDeselection: true,
            columnDefs: columnDefs,
            // tell grid we want virtual row model type
            rowModelType: 'infinite',
            // how big each page in our page cache will be, default is 100
            paginationPageSize: 100,
            // how many extra blank rows to display to the user at the end of the dataset,
            // which sets the vertical scroll and then allows the grid to request viewing more rows of data.
            // default is 1, ie show 1 row.
            cacheOverflowSize: 2,
            // how many server side requests to send at a time. if user is scrolling lots, then the requests
            // are throttled down
            maxConcurrentDatasourceRequests: 2,
            // how many rows to initially show in the grid. having 1 shows a blank row, so it looks like
            // the grid is loading from the users perspective (as we have a spinner in the first col)
            infiniteInitialRowCount: 1,
            // how many pages to store in cache. default is undefined, which allows an infinite sized cache,
            // pages are never purged. this should be set for large data to stop your browser from getting
            // full of data
            maxBlocksInCache: 2,
            onGridReady: function () {
                vm.kadPrikaz();
            }
        };

        vm.kadUnos = function () {
            vm.unos = true;
            vm.prikaz = false;
        };

        function setRowData() {
            var dataSource = {
                rowCount: null, // behave as infinite scroll
                getRows: function (params) {
                    console.log('asking for ' + params.startRow + ' to ' + params.endRow);
                    // At this point in your code, you would call the server, using $http if in AngularJS 1.x.
                    // To make the demo look real, wait for 500ms before returning
                    RecordService.GetAll(params.startRow, 100)
                        .then(function (response) {
                            vm.count = response.count;
                            vm.records = response.records;

                            // vm.rowData = [{ recordId: 1, bx: 2, by: 4, ax: 4, ay: 5 }];

                            // take a slice of the total rows
                            var rowsThisPage = vm.records.slice(params.startRow, params.endRow);
                            // if on or after the last page, work out the last row.
                            var lastRow = -1;
                            if (vm.count <= params.endRow) {
                                lastRow = vm.records.length;
                            }
                            // call the success callback
                            params.successCallback(rowsThisPage, lastRow);
                        });
                }
            };

            vm.gridOptions.api.setDatasource(dataSource);
            vm.gridOptions.api.sizeColumnsToFit();

        }

        vm.kadPrikaz = function () {
            vm.unos = false;
            vm.prikaz = true;

            setRowData(vm.records);

        };

        vm.sacuvaj = function () {
            if (validan(vm.podatak)) {
                if (vm.izmena) {
                    var data = [];
                    data.push(vm.podatak);
                    RecordService.UpdateRecord(data).then(function (response) {
                        console.log(response);
                    });

                    vm.podatak = {};
                    vm.izmena = false;
                }
                else {
                    RecordService.AddRecord(vm.podatak)
                        .then(function (response) {
                            var isError = response.success === false ? true : false;
                            if (!isError) {
                                console.log(response);
                                toastr.success('Podatak je dodat', 'Success', {"timeOut": 5000, "positionClass": "toast-top-center"});
                                vm.podatak = {};
                            }
                            else {
                                toastr.error(response.message, "Error", {
                                    "timeOut": 5000
                                });
                            }
                        });
                }

            }
            else {
                toastr.error('Niste popunili sva polja');
            }
        };

        var validan = function (data) {

            if (data.Bx && data.By && data.Ax && data.Ay) {
                return true;
            }

            return false;
        };


        vm.obrisiSve = function () {
            RecordService.DeleteAllRecords()
                .then(function (response) {
                    vm.kadPrikaz();
                });
        };

        function uzmiId(rows) {
            var niz = [];
            rows.forEach(function (element) {
                niz.push(element.recordId);
            });
            return niz;
        }

        vm.obrisi = function () {
            var rows = vm.gridOptions.api.getSelectedRows();
            rows = uzmiId(rows);
            RecordService.DeleteRecords(rows)
                .then(function (response) {
                    vm.kadPrikaz();
                });
        };

        vm.izmeniRed = function () {
            vm.izmena = true;
            var rows = vm.gridOptions.api.getSelectedRows();
            vm.unos = true;
            vm.prikaz = false;
            vm.podatak.Bx = rows[0].bx;
            vm.podatak.By = rows[0].by;
            vm.podatak.Ax = rows[0].ax;
            vm.podatak.Ay = rows[0].ay;
            vm.podatak.RecordId = rows[0].recordId;
        };

        vm.download = function () {
            document.getElementById("downloadFile").submit();
        };

        vm.dodajExcel = function () {
            var input_file = document.getElementById("file");
            input_file.addEventListener("change", function () {
                if (this.files.length > 0) {
                    var form = new FormData();
                    form.append("file", this.files[0]);
                    $http({
                        method: "POST",
                        url: "api/records/file",
                        data: form,
                        headers: {
                            "Content-Type": "application/octet-stream"
                        }
                    }, function () {
                        console.log("Success");
                    }, function () {
                        console.log("Error");
                    });
                }
            });
            input_file.click();
        };
    }

})();