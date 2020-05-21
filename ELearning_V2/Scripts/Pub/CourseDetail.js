var CourseDetailApp = angular.module("CourseDetailApp", ['angularUtils.directives.dirPagination']);

CourseDetailApp.directive('ngFiles', ['$parse', function ($parse) {
    function fn_link(scope, element, attrs) {
        var onChange = $parse(attrs.ngFiles);
        element.on('change', function (event) {
            onChange(scope, { $files: event.target.files });
        });
    };

    return {
        link: fn_link
    }
}])

CourseDetailApp.controller('CourseDetailController', function ($scope, $http, $window, CourseDetailService) {
    runWaiting();
    var formdata = new FormData();
    $scope.ID = function (id) {
        $scope.LcurrentPage = 1;
        $scope.LpageSize = 5;
        $scope.McurrentPage = 1;
        $scope.MpageSize = 5;
        $scope.LTAcurrentPage = 1;
        $scope.LTApageSize = 5;
        $scope.TcurrentPage = 1;
        $scope.TPageSize = 5;
        $scope.CourseID = id;
        $scope.Username = '';
        $scope.Five = 0;
        $scope.Four = 0;
        $scope.Three = 0;
        $scope.Two = 0;
        $scope.One = 0;
        $scope.Total = 0;
        $scope.TestIDEdit = 0;
        LoadClass(id);
        LoadMember(id);
        LoadLession(id);
        LoadLessionToAdd(id);
        LoadComment(id);
        LoadTest(id);
        downWaiting();
    };
    function runWaiting() {
        console.log("Open");
        angular.element("#main-wait").css("display", "block");
        angular.element("body").css("overflow", "hidden");
    }
    function downWaiting() {
        console.log("Close");

        angular.element("#main-wait").css("display", "none");
        angular.element("body").css("overflow", "auto");
    }
    function LoadClass(ID) {
        CourseDetailService.LoadClass(ID).then(function (d) {
            $scope.Lop = d.data;
            console.log(d.data);
        }, function () {
            alert('Không tìm thấy dữ liệu !!!');
        });
    }
    function LoadMember(ID) {
        CourseDetailService.LoadMember(ID).then(function (d) {
            $scope.Members = d.data;
            console.log(d.data);
        }, function () {
            alert('Không tìm thấy dữ liệu !!!');
        });
    }
    function LoadLession(ID) {
        CourseDetailService.LoadLession(ID).then(function (d) {
            $scope.Lessions = d.data;
            console.log(d.data);
        }, function () {
            alert('Không tìm thấy dữ liệu !!!');
        });
    }
    function LoadLessionToAdd(ID) {
        CourseDetailService.LoadLessionToAdd(ID).then(function (d) {
            $scope.LessionToAdds = d.data;
            console.log(d.data);
        }, function () {
            alert('Không tìm thấy dữ liệu !!!');
        });
    }
    function LoadComment(ID) {
        var CommentDTO = {
            CourseID: ID
        };
        CourseDetailService.LoadComment(CommentDTO).then(function (d) {
            var temp = d.data;
            if (temp.length != 0) {
                for (var i = 0; i < temp.length; i++) {
                    if (temp[i]["Rate"] == 1) {
                        $scope.One++;
                    }
                    if (temp[i]["Rate"] == 2) {
                        $scope.Two++;
                    }
                    if (temp[i]["Rate"] == 3) {
                        $scope.Three++;
                    }
                    if (temp[i]["Rate"] == 4) {
                        $scope.Four++;
                    }
                    if (temp[i]["Rate"] == 5) {
                        $scope.Five++;
                    }
                    temp[i]["CreateDate"] = new Date(parseInt(temp[i]["CreateDate"].substr(6)));
                    for (var j = 0; j < temp[i]["Replies"].length; j++) {
                        temp[i]["Replies"][j]["CreateDate"] = new Date(parseInt(temp[i]["Replies"][j]["CreateDate"].substr(6)));
                    }
                }
                $scope.Total = ((1 * $scope.One) + (2 * $scope.Two) + (3 * $scope.Three) + (4 * $scope.Four) + (5 * $scope.Five)) / temp.length;
                $scope.FTotal = Math.floor($scope.Total);
                var bar1 = document.getElementById('bar1');
                bar1.style.width = ($scope.One / temp.length) * 100 + "%";
                var bar2 = document.getElementById('bar2');
                bar2.style.width = ($scope.Two / temp.length) * 100 + "%";
                var bar3 = document.getElementById('bar3');
                bar3.style.width = ($scope.Three / temp.length) * 100 + "%";
                var bar4 = document.getElementById('bar4');
                bar4.style.width = ($scope.Four / temp.length) * 100 + "%";
                var bar5 = document.getElementById('bar5');
                bar5.style.width = ($scope.Five / temp.length) * 100 + "%";
            }
            $scope.Comments = temp;
            temp = null;
            console.log($scope.Comments);

        }, function () {
            alert('Không tìm thấy dữ liệu !!!');
        });
    }
    function LoadTest(ID) {
        CourseDetailService.LoadTest(ID).then(function (d) {
            if (d.data == 0) {
                alert("Không đủ quyền hạn");
            }
            else {
                if (d.data == -1) {
                    alert("Không có dữ liệu");
                }
                else {
                    $scope.Tests = d.data;
                    for (var i = 0; i < d.data.length; i++) {
                        $scope.Tests[i].CreateDate = new Date(parseInt($scope.Tests[i].CreateDate.substr(6)));
                    }
                    console.log(d.data);
                }
            }
        }, function () {
            alert('Không tìm thấy dữ liệu !!!');
        });
    }
    function AddMember(Username) {
        var CourseDetailDTO = {
            CourseID: $scope.CourseID,
            Username: Username
        }
        CourseDetailService.AddMember(CourseDetailDTO).then(function (d) {
            LoadMember($scope.CourseID);
            $scope.Username = '';
            alert(d.data);
        }, function () {
            alert('Không tìm thấy dữ liệu !!!');
        });
    }
    function FindLession(ID) {
        for (var i = 0; i < $scope.Lessions.length; i++) {
            if ($scope.Lessions[i].ID == ID) {
                return i;
            }
        }
    }
    function FindLessionToAdd(ID) {
        for (var i = 0; i < $scope.LessionToAdds.length; i++) {
            if ($scope.LessionToAdds[i].ID == ID) {
                return i;
            }
        }
    }
    function FindTest(ID) {
        for (var i = 0; i < $scope.Tests.length; i++) {
            if ($scope.Tests[i].ID == ID) {
                return i;
            }
        }
    }
    $scope.EditCourse = function () {
        console.log('i am inside update funcr ' +
            JSON.stringify($scope.Lop));
        $http({
            method: 'POST',
            url: '/Lop/EditCourse',
            data: JSON.stringify($scope.Lop)
        }).then(function successCallback(response) {
            LoadClass($scope.Lop.ID);
            alert(response.data);
        }, function errorCallback(response) {
            alert("Error : " + response.data.ExceptionMessage);
        });
    };
    $scope.Add = function () {
        var isExpanded = $('#Add').hasClass('show');
        if (isExpanded && $scope.Username != '') {
            AddMember($scope.Username);
        }
    }
    $scope.Info = function (User) {
        $scope.User = User;
    }
    $scope.RemoveMember = function (UserID) {
        var CourseDetailDTO = {
            CourseID: $scope.CourseID,
            UserID: UserID
        };
        $http({
            method: 'POST',
            url: '/Lop/RemoveMember',
            data: JSON.stringify(CourseDetailDTO)
        }).then(function successCallback(response) {
            LoadMember($scope.CourseID);
            alert(response.data);
        }, function errorCallback(response) {
            alert("Error : " + response.data.ExceptionMessage);
        });
    }
    $scope.ChangeLessionStatus = function (ID, Status) {
        var LessionlDTO = {
            ID: ID,
            CourseID: $scope.CourseID,
            Course_LessionStatus: Status
        };
        CourseDetailService.ChangeLessionStatus(LessionlDTO).then(function (d) {
            LoadLession($scope.CourseID);
            alert(d.data);
        }, function () {
            alert('Không tìm thấy dữ liệu !!!');
        });
    }
    $scope.ViewLession = function (LessionID) {
        $window.location.href = '/Lop/LessionDetail/' + LessionID + '?CourseID=' + $scope.CourseID;

        //$http({
        //    method: 'GET',
        //    url: '/Lop/LessionDetail/' + LessionID
        //}).then(function successCallback(response) {
        //    alert(response.data);
        //    if (response.data == 0) {
        //        alert("Không tìm thấy bài giảng");
        //    }
        //    else {
        //        if (response.data == -1) {
        //            alert("Không không đủ quyền hạn")
        //        }
        //        else {
        //        }
        //    }
        //});

    }
    $scope.EditLession = function (ID) {
        $window.location.href = '/Lop/EditLession/' + ID;

        //$http({
        //    method: 'GET',
        //    url: '/Lop/EditLession/' + ID
        //}).then(function successCallback(response) {
        //    if (response.data == 0) {
        //        alert("Không tìm thấy bài giảng");
        //    }
        //    else {
        //        if (response.data == -1) {
        //            alert("Không không đủ quyền hạn")
        //        }
        //        else {
        //        }
        //    }
        //});
    }
    $scope.RemoveLession = function (ID) {
        var LessionlDTO = {
            ID: ID,
        };
        CourseDetailService.RemoveLession(LessionlDTO).then(function (d) {
            LoadLession($scope.CourseID);
            alert(d.data);
        }, function () {
            alert('Không tìm thấy dữ liệu !!!');
        });
    }
    $scope.AddLession = function () {
        CourseDetailService.LoadLessionToAdd($scope.CourseID).then(function (d) {
            console.log(d.data);
            $scope.LessionToAdds = d.data;
        }, function () {
            alert('Không tìm thấy dữ liệu !!!');
        });
        //$http({
        //    method: 'GET',
        //    url: '/Lop/CreateLession/' + $scope.CourseID
        //}).then(function successCallback(response) {
        //    if (response.data == 0) {
        //        alert("Không tìm thấy lớp học");
        //    }
        //    else {
        //        if (response.data == -1) {
        //            alert("Không không đủ quyền hạn")
        //        }
        //        else {
        //            $window.location.href = '/Lop/CreateLession/' + $scope.CourseID;
        //        }
        //    }
        //});
    }
    $scope.AddLessionToCourse = function (LessionID) {
        var LessionDTO = {
            ID: LessionID,
            CourseID: $scope.CourseID
        };
        $http({
            method: 'POST',
            url: '/Lop/AddLessionToCourse',
            data: JSON.stringify(LessionDTO)
        }).then(function successCallback(response) {
            if (response.data != null) {
                //LoadLessionToAdd($scope.CourseID);
                LoadLession($scope.CourseID);
                //console.log(response.data)
                $scope.LessionToAdds.splice(FindLessionToAdd(LessionID), 1);
                //$scope.Lessions.push(response.data);
                alert("Đã thêm");
            }
        });
    }
    $scope.RemoveLessionFromCourse = function (LessionID) {
        var LessionDTO = {
            ID: LessionID,
            CourseID: $scope.CourseID
        };
        $http({
            method: 'POST',
            url: '/Lop/RemoveLessionFromCourse',
            data: JSON.stringify(LessionDTO)
        }).then(function successCallback(response) {
            if (response.data != null) {
                LoadLessionToAdd($scope.CourseID);
                //LoadLession($scope.CourseID);
                $scope.Lessions.splice(FindLessionToAdd(LessionID), 1);
                //$scope.LessionToAdds.push(response.data);
                alert("Đã xóa");
            }
        });
    }
    $scope.ViewTest = function (TestID) {
        $window.location.href = '/Lop/TestDetail/' + TestID + '?CourseID=' + $scope.CourseID;
    }
    $scope.CreateTest = function () {
        var isHidden = $('#AddTestForm').is(":hidden");
        if (isHidden == false && $scope.TestName != null && $scope.AmountQuestion != null && $scope.TestTime != null) {
            if ($scope.TestIDEdit == 0) {
                console.log("Add")
                var TestDTO = {
                    Name: $scope.TestName,
                    CourseID: $scope.CourseID,
                    AmountQuestion: $scope.AmountQuestion,
                    Time: $scope.TestTime
                }
                console.log(TestDTO);
                $http({
                    method: 'POST',
                    url: '/Lop/CreateTest',
                    data: JSON.stringify(TestDTO)
                }).then(function successCallback(response) {
                    if (response.data == 1) {
                        LoadTest($scope.CourseID);
                        alert("Đã thêm");
                        $scope.TestName = null;
                        $scope.AmountQuestion = null;
                        $scope.TestTime = null
                    }
                    if (response.data == -1) {
                        alert("Xảy ra lỗi");
                    }
                });
            }
            else {
                console.log("Edit")
                var TestDTO = {
                    ID: $scope.TestIDEdit,
                    Name: $scope.TestName,
                    AmountQuestion: $scope.AmountQuestion,
                    Time: $scope.TestTime

                }
                console.log(TestDTO);
                $http({
                    method: 'POST',
                    url: '/Lop/EditTest',
                    data: JSON.stringify(TestDTO)
                }).then(function successCallback(response) {
                    if (response.data == 1) {
                        LoadTest($scope.CourseID);
                        alert("Đã cập nhật");
                        $scope.TestName = null;
                        $scope.AmountQuestion = null;
                        $scope.TestTime = null

                    }
                    if (response.data == -1) {
                        alert("Xảy ra lỗi");
                    }
                });

            }
        }
    }
    $scope.EditTest = function (TestID) {
        var isHidden = $('#AddTestForm').is(":hidden");
        if (isHidden == true) {
            $('#AddTestForm').collapse("toggle")
            $scope.TestName = $scope.Tests[FindTest(TestID)].Name;
            $scope.AmountQuestion = $scope.Tests[FindTest(TestID)].AmountQuestion;
            $scope.TestIDEdit = TestID;
            $scope.TestTime = $scope.Tests[FindTest(TestID)].Time;
        }
        else {
            $scope.TestName = $scope.Tests[FindTest(TestID)].Name;
            $scope.AmountQuestion = $scope.Tests[FindTest(TestID)].AmountQuestion;
            $scope.TestTime = $scope.Tests[FindTest(TestID)].Time;
            $scope.TestIDEdit = TestID;
        }
    }
    $scope.DeleteTest = function (TestID) {
        var TestDTO = {
            ID: TestID
        }
        $http({
            method: 'POST',
            url: '/Lop/DeleteTest',
            data: JSON.stringify(TestDTO)
        }).then(function successCallback(response) {
            if (response.data == 1) {
                LoadTest($scope.CourseID);
                alert("Đã xóa");
            }
            if (response.data == -1) {
                alert("Xảy ra lỗi");
            }
        });
    }
    $scope.ChangeTestStatus = function (TestID, Status) {
        var TestDTO = {
            ID: TestID,
            Status: Status
        }
        $http({
            method: 'POST',
            url: '/Lop/ChangeTestStatus',
            data: JSON.stringify(TestDTO)
        }).then(function successCallback(response) {
            if (response.data == 1) {
                LoadTest($scope.CourseID);
            }
            if (response.data == -1) {
                alert("Xảy ra lỗi");
            }
        });
    }
    $scope.getTheFiles = function ($files) {
        console.log($files);
        file = $files[0];
        console.log(formdata);

        var blob = file.slice(0, file.size, 'image/jpg');
        newFile = new File([blob], $scope.CourseID + '.jpg', { type: 'image/jpg' });
        console.log('filename ' + newFile);
        formdata.set(0, newFile);
        console.log(formdata.get(0));
        $http({
            method: 'POST',
            url: '/api/API/UploadClassImage',
            data: formdata,
            headers: {
                'Content-Type': undefined
            }
        }).then(function successCallback(response) {
            alert(response.data);
        });
    };






    $scope.lop = {
        Name: '',
        Capacity: null,
        Description: '',
        Image: '',
        Price: null,
        Schedule: '',
        Condition: '',
        Type: null
    }
    $scope.DangKy = function (type) {
        if (type == 1) {
            $scope.lop.Type = 1;
            $scope.lop.Capacity = 15;
            $scope.lop.Price = 0;
        }
        if (type == 2) {
            $scope.lop.Type = 2;
            $scope.lop.Capacity = 45;
        }
        if (type == 3) {
            $scope.lop.Type = 3;
            $scope.lop.Capacity = null;
        }
        $scope.type = type;
    }
    $scope.Save = function ($files) {
        var ID;
        console.log('Save' + JSON.stringify($scope.lop));
        $http({
            method: 'POST',
            url: '/Lop/CreateClass',
            data: JSON.stringify($scope.lop)
        }).then(function successCallback(response) {
            ID = response.data;
            console.log(ID);
            //$scope.Clear();
            console.log('filename ' + file.name);
            var blob = file.slice(0, file.size, 'image/jpg');
            newFile = new File([blob], ID + '.jpg', { type: 'image/jpg' });
            console.log('filename ' + newFile);
            formdata.set(0, newFile);
            console.log(formdata.get(0));
            var request = {
                method: 'POST',
                url: '/api/API/',
                data: formdata,
                headers: {
                    'Content-Type': undefined
                }
            };
            $http(request)
                .success(function (d) {
                    alert(d);
                    LoadClass();
                })
                .error(function () {
                });

        }, function errorCallback(response) {
            alert("Error : " + response.data.ExceptionMessage);
        });
    }
    $scope.Clear = function () {
        $scope.lop.Name = '';
        $scope.lop.Capacity = null;
        $scope.lop.Description = '';
        $scope.lop.Image = '';
        $scope.lop.Price = null;
        $scope.lop.Schedule = '';
        $scope.lop.Condition = '';
        $scope.lop.Type = null;
    }
    $scope.View = function (ID) {
        alert(ID);
    }
});




CourseDetailApp.factory('CourseDetailService', function ($http) {
    var fac = {};
    fac.LoadClass = function (ID) {
        return $http.get('/Lop/GetClassByID/' + ID);
    };
    fac.LoadMember = function (ID) {
        return $http.get('/Lop/GetMemberByClassID/' + ID);
    };
    fac.LoadLession = function (ID) {
        return $http.get('/Lop/GetLessionByClassID/' + ID);
    };
    fac.LoadLessionToAdd = function (ID) {
        return $http.get('/Lop/LoadLessionToAdd/' + ID);
    };
    fac.LoadTest = function (ID) {
        return $http.get('/Lop/GetListTestByCourseID/' + ID);
    };
    fac.AddMember = function (CourseDetailDTO) {
        return $http({
            method: 'POST',
            url: '/Lop/AddMember',
            data: JSON.stringify(CourseDetailDTO)
        });
    };
    fac.ChangeLessionStatus = function (LessionDTO) {
        return $http({
            method: 'POST',
            url: '/Lop/ChangeLessionStatus',
            data: JSON.stringify(LessionDTO)
        });
    };
    fac.RemoveLession = function (LessionDTO) {
        return $http({
            method: 'POST',
            url: '/Lop/RemoveLession',
            data: JSON.stringify(LessionDTO)
        });
    };
    fac.LoadComment = function (CommentDTO) {
        return $http({
            method: 'POST',
            url: '/Lop/GetCommentByID',
            data: JSON.stringify(CommentDTO)
        });
    };
    return fac;
});

