/*
    Document ready event has been commented out to account for an issue in IE 9 compatibility mode
    It is expected this should be solved when they introduce IE 11 however this will need to be tested
    Until then this JavaScript needs to remain at the foot of the page after the HTML has rendered so we
    know the HTML is actually on the page before we start to manipulate elements
    */
function loadDropdowns() {
    // $(document).ready(function () {
    // Get the Directorate information for the first level drop down list
    $.getJSON('http://ekd-web-03/MIAPI/MI/AllDirectorates?type=json', function (dirData) {
        var $select = $('#dirSelect');
        $optionAll = "<option value=\"\">DFID All</option>";
        $select.append($optionAll);
        for (var i in dirData) {
            var $option = "<option value=\"" + dirData[i].DirectorGeneralCode + "\">" + dirData[i].DirectorGeneralName + "</option>";

            $select.append($option);
        }
    });
}

function clearDropdownsDir() {
    $('#divSelect').empty().html('<option value="">Select a Division</option>');
    $('#deptSelect').empty().html('<option value="">Select a Department</option>');
}

function clearDropdownsDiv() {
   $('#deptSelect').empty().html('<option value="">Select a Department</option>');
}

// Function which handles the drop down selection for Director General level
function doDirSelect(dir){
    clearDropdownsDir();
    //document.getElementById("divSelect").innerHTML = '<option value="">Select a Division</option>';
     //document.getElementById("deptSelect").innerHTML = '<option value="">Select a Department</option>';
    if (dir != "" && dir != null) {
        $.getJSON('http://ekd-web-03/MIAPI/MI/GetDivByDG/' + dir +'?type=json', function (dirData) {
           var $select = $('#divSelect');
           for (var i in dirData) { 
                var $option = "<option value=\"" + dirData[i].DivisionCode +"\">" + dirData[i].DivisionName + "</option>";
						
                $select.append($option);
            }
        });
        document.getElementById("divSelect").disabled = false;
        document.getElementById("deptSelect").disabled = true;
        updateFinance(dir);
        updateProjects(dir);
        updatePQ(dir);
        //clearGraph();
        //drawGraph(dir);
        drawBudgetChart(dir);
        drawBudgetChartThisFY(dir);
        drawBudgetChartCumulative(dir);
    }
    else {
        updateFinance(defaultOU);
        updateProjects(defaultOU);
        updatePQ(defaultOU);
        //clearGraph();
        //drawGraph(defaultOU);
        drawBudgetChart(defaultOU);
        drawBudgetChartThisFY(defaultOU);
        drawBudgetChartCumulative(defaultOU);
    }
}
// Function which handles the drop down selection for Divisional level
function doDivSelect(div){
    clearDropdownsDiv();
    //document.getElementById("deptSelect").innerHTML = '<option value="">Select a Department</option>';
    if (div != "" && div != null) {
        $.getJSON('http://ekd-web-03/MIAPI/MI/GetDeptByDiv/' + div +'?type=json', function (dirData) {
            var $select = $('#deptSelect');
            for (var i in dirData) { 
                var $option = "<option value=\"" + dirData[i].DeptOffCode +"\">" + dirData[i].DeptOffName + "</option>";
						
                $select.append($option);
            }
        });
        document.getElementById("deptSelect").disabled = false;
        updateFinance(div);
        updateProjects(div);
        updatePQ(div);
        //clearGraph();
        //drawGraph(div);
        drawBudgetChart(div);
        drawBudgetChartThisFY(div);
        drawBudgetChartCumulative(div);
    }
}
// Function which handles the drop down selection for Department Office level
function doDeptSelect(dept){
			
    if (dept != "" && dept != null) {
					
        updateFinance(dept);
        updateProjects(dept);
        updatePQ(dept);
        //clearGraph();
        //drawGraph(dept);
        drawBudgetChart(dept);
        drawBudgetChartThisFY(dept);
        drawBudgetChartCumulative(dept);
    }
}
// Set up the info pop ups
$(".more-info-link").click(function(e){
    //$(".more-info").hide();
    e.stopPropagation();
    $("#moreinfo"+$(this).attr('target')).toggle();
});
			 
$(".more-info").on('click', function(e){
    e.stopPropagation();
});

$(document).on('click', function(e){
    $(".more-info").hide(); 
});

//build the margins of the container for the graph
var margin = { top: 20, right: 50, bottom: 30, left: 150 },
    width = 900 - margin.left - margin.right,
    height = 300 - margin.top - margin.bottom;

// Create the variables that will be used in the graph
var x = d3.scale.ordinal()
    .rangeRoundBands([0, width], .1, 0);

var y = d3.scale.linear().range([height, 0]);

var xAxis = d3.svg.axis().scale(x)
    .orient("bottom").ticks(5);

var yAxis = d3.svg.axis().scale(y)
    .orient("left").ticks(5);

var spendarea = d3.svg.area()
    .x(function (d) { return x(d.FiscalPeriod); })
    .y0(height)
    .y1(function (d) { return y(d.SpendValueCumulative); });

var spendline = d3.svg.line()
    .x(function (d) { return x(d.FiscalPeriod); })
    .y(function (d) { return y(d.SpendValueCumulative); });
 
var budgetline = d3.svg.line()
     .x(function (d) { return x(d.FiscalPeriod); })
     .y(function (d) { return y(d.BudgetValueCumulative); });
	
		
// The drawing of the graph has been wrapped up into a function so that it can be updated in-line with the level of drill down
function drawGraph(OUCode){
	
		
    // append svg to the right dom location
    var svg = d3.select("div.migraphlocation")
        .append("svg")
            .attr("width", width + margin.left + margin.right)
            .attr("height", height + margin.top + margin.bottom)
        .append("g")
            .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

    // Get the data
			
    d3.json('http://ekd-web-03/MIAPI/MI/CurrentBudget/' + OUCode + '?type=json', function (error, data) {
				
				
        // Set-up a subsection of actual spending array
        var data2 = new Array();
        for (var i in data) {
            if (data[i].FiscalYear == thisFY()) {
                if ((data[i].SpendValue > 0) || (data[i].SpendValue < 0)) {
                    data2.push(data[i]);
                }
            }
            data2.sort(function (a, b) {
                return a.FiscalPeriod - b.FiscalPeriod;
            });
        }

        //set up the array so it covers this FY only
        var thisFYdata = new Array();
        for (var i in data) {
            if (data[i].FiscalYear == thisFY()) {
                thisFYdata.push(data[i]);
            }
            thisFYdata.sort(function (a, b) {
                return a.FiscalPeriod - b.FiscalPeriod;
            });
        }
        //// Set-up the 90% of budget line
        //	var targetline = d3.svg.line()
        //     .x(function (d) { return x(d.FiscalPeriod); })
        //     .y(function (d) { return y(target * 0.9); }); // target is based on the resource budget from the financial spend AJAX call but can be outwith the bounds of the data being added to the graph
        //// Set-up month 9 target line	
        //	var targetline2 = d3.svg.line()
        //     .x(function (d) { return x(9); })
        //     .y(function (d) { return y(d.BudgetValueCumulative); });
			
        // Start creating the graph
        x.domain(thisFYdata.map(function (d) { return d.FiscalPeriod; }));
        y.domain([0, d3.max(thisFYdata, function (d) { return d.BudgetValueCumulative; })]);

        svg.append("path")
            .datum(data2)
            .attr("class", "area")
            .attr("d", spendarea);

        svg.append("path")      // Add the valueline path.
            .attr("d", spendline(data2));
        svg.append("path")      // Add the valueline path.
             .attr("d", budgetline(thisFYdata))
             .attr("class", "path2");
				
        //svg.append("path")      // Add the targetline amount path.
        //     .attr("class", "line")
        //	 .style("stroke-opacity", .6) // line opacity
        //	 .style("stroke", "grey") // line colour
        //	 .style("stroke-dasharray", ("3, 3"))
        //     .attr("d", targetline(data));
        //svg.append("path")      // Add the targetline by period 9 path.
        //     .attr("class", "line")
        //	 .style("stroke-dasharray", ("3, 3"))
        //	 .style("stroke-opacity", .6) // line opacity
        //	 .style("stroke", "grey") // line colour
        //     .attr("d", targetline2(data));
				
        svg.append("g")         // Add the X Axis
            .attr("class", "x axis")
            .attr("transform", "translate(-25," + height + ")")
            .call(xAxis);

        svg.append("g")         // Add the Y Axis
            .attr("class", "y axis")
            .call(yAxis);
				
    });
}
/*
    Set-up the default screen using the defaultOU of DFID
    */
loadDropdowns();
updateFinance(defaultOU);
updateProjects(defaultOU);
updatePQ(defaultOU);
//drawGraph(defaultOU);
drawBudgetChart(defaultOU);
drawBudgetChartThisFY(defaultOU);
drawBudgetChartCumulative(defaultOU);
//fillTable(defaultOU);
/*
    Small function to clear the graph in between redrawing as the graph mainly uses append
    on its own it would keep adding rather than refreshing with new data
    */
function clearGraph(){
    var str = '<div class="Legend">\n'
               + '<div class="LegendLabel">Cumulative Budget</div>&nbsp;<div class="Budget"></div>\n'
               + '<div class="LegendLabel">Cumulative Spend</div>&nbsp;<div class="Spend"></div>\n'
               + '<div class="LegendLabel target">90&#37; Target of total budget by period 9</div>\n'
               + '</div>\n'
            + '<p class="source">Source: ARIES</p>\n';
    document.getElementById("graph").innerHTML = str;
}

function thisFY() {
    var d = new Date();
    var month = d.getMonth();
    var year = d.getFullYear();
    if (month < 3)
        return year - 1;
    else
        return year;
}

var approvedResourceBudget;
/*
    Update routines for the financial spend, projects & portfolio quality to match drill down functionality
    */
function updateFinance(OUCode){
    $.getJSON('http://ekd-web-03/MIAPI/MI/FinancialYearSpend/' + OUCode + '?type=json', function (data) {
        var d = new Date();                        
        //var thisYear = d.getFullYear()-1;
        var thisYear = thisFY();
        var lastYear = thisYear - 1;
        var lastYearsSpend;
        var thisYearsSpend;
        var thisYearTitle;
        var percentOfTarget;
						
        for (var i in data) {
            if (data[i].FiscalYear == lastYear){
                lastYearsSpend = (data[i].Spend / 1000000).toFixed(1);						
            } else if (data[i].FiscalYear == thisYear) {
                thisYearsSpend = (data[i].Spend / 1000000).toFixed(1);
                thisYearTitle = data[i].FiscalYearLabel;
                target = data[i].ResourceBudget;
                //alert(thisYearsSpend);
                //alert(target);
                if (thisYearsSpend != "undefined" && target != "undefined") {
                    percentOfTarget = ((thisYearsSpend / (target / 1000000).toFixed(1)) * 100).toFixed(1);
                } else {
                    percentOfTarget = "No Data";
                }
            }
								
        }
							
        var percentoflastyear = ((thisYearsSpend / lastYearsSpend) * 100).toFixed(1);	
        document.getElementById("spendThisYear").innerHTML = "&pound;" + numberWithCommas(thisYearsSpend) + " m";
        if (data[i]){
            document.getElementById("percentOfTarget").innerHTML = "<strong>" + percentOfTarget +"&#37;</strong> of &pound;" + numberWithCommas((target / 1000000).toFixed(1)) + "m resource budget in " + thisYearTitle;
        } else {
            document.getElementById("percentOfTarget").innerHTML = "No Data";
        }

        approvedResourceBudget = target;

    });
}
function updateProjects(OUCode){
    $.getJSON('http://ekd-web-03/MIAPI/MI/ProjStageCount/' + OUCode + '?type=json', function (data) {
        if (data[0]){
            var dataMartVersion = data[0].DataMartVersion;
            document.getElementById("dmVersion").innerHTML = dataMartVersion;
            var d = new Date();
            document.getElementById("buildDate").innerHTML = d.toDateString();
            var numProjects = 0;
            var numPipelineProjects = 0;
            for (var i in data) {
                if (data[i].StageName != "Completion" && data[i].StageName != "Post Completion" && data[i].StageName != "Implementation" && data[i].StageName != "Pre Pipeline"){
                    numPipelineProjects = numPipelineProjects + data[i].NumberOfProjects;
                }
                if (data[i].StageName == "Implementation"){
                    numProjects = data[i].NumberOfProjects;
                }
            }
            document.getElementById("numberofprojects").innerHTML = numberWithCommas(numProjects);
            document.getElementById("numberofpipeline").innerHTML = numberWithCommas(numPipelineProjects);
        } else {
            document.getElementById("numberofprojects").innerHTML = "No Data";
            document.getElementById("numberofpipeline").innerHTML = "No Data";
        }
    });
}
function updatePQ(OUCode){
    $.getJSON('http://ekd-web-03/MIAPI/MI/GetOUPQ/' + OUCode + '?type=json', function (data) {
        if (data[0]){
            var portfolioQuality = data[0].PQScore.toFixed(1);
            document.getElementById("portfolioQuality").innerHTML = portfolioQuality;
        } else {
            document.getElementById("portfolioQuality").innerHTML = "No Data"; 
        }
    });
}
// });
	

//budgetchart
var thisFYdata;

function drawBudgetChartThisFY(OUCode) {
    $.getJSON('http://ekd-web-03/MIAPI/MI/CurrentBudget/' + OUCode + '?type=json', function (json) {

        var data = new Array();

        //pull out this FY only
        json.forEach(function (d) {
            if (d.FiscalYear == thisFY()) {
                data.push(d);
            }
        });
        thisFYdata = data;

        //sort the data first
        data.sort(function (a, b) {
            return a.FiscalPeriod - b.FiscalPeriod;
        });
        //console.log(data);

        //make individual arrays
        periods = $.map(data, function (d) {
            return d.FiscalPeriod;
        });
        //console.log(periods);

        budgets = $.map(data, function (d) {
            return d.BudgetValueCumulative;
        });

        spend = $.map(data, function (d) {
            if (d.SpendValue >0)
                return d.SpendValueCumulative;
        });
        console.log(budgets);

        var chart = c3.generate({
            bindto: '#budgetchartThisFY',
            padding: { left: 50 },
            data: {
                x: 'x',
                columns: [
                   ['x'].concat(periods),
                   ['budget'].concat(budgets),
                   ['spend'].concat(spend)
                ],
                names: {
                    budget: 'Approved project budget',
                    spend: 'Spend'
                },
                colors: {
                    budget: 'darkred',
                    spend: 'green'
                },
                types: {
                    spend: 'bar'
                }
            },
            axis: {
                x: {
                    tick: {
                        culling: false,
                        format: function (x) { return getMonth(x); }
                    },
                    label: '2014/15 period'
                },
                y: {
                    label: 'GBP millions',
                    //tick: { format: d3.format(',d') },
                    tick: {
                        format: function (x) { return getMillions(x); }
                    },
                    padding: { bottom: 0 },
                    min: 0
        }
            },
            grid: {
               y: {
                   lines: [{ value: approvedResourceBudget, text: 'Resource Allocation: GBP ' + approvedResourceBudget/1000000 + 'm'}]
                }
            },
    legend: {
            position: 'inset',
            inset: {
            anchor: 'top-left',
            x: 20,
            y: 50,
            step: undefined
            }
    },
    tooltip: {
            format: {
                title: function (d) { return 'Period ' + d + ' (' + getMonth(d) + ')'; },
                value: function (value, ratio, id) {
                    //var format = id === 'budget' ? d3.format(',') : d3.format('£');
                    var format = d3.format(',.0f');
                    return '&pound;' + format(value);
                }
            }
    }

        });
    });
    document.getElementById("budgetchartThisFYlink").innerHTML = chartDataURL(OUCode);
}

function drawBudgetChart(OUCode) {
    $.getJSON('http://ekd-web-03/MIAPI/MI/CurrentBudget/' + OUCode + '?type=json', function (data) {

        //var data = new Array();

        //pull out this FY only
        //json.for(function (d) {
        //    if (d.FiscalYear == thisFY() + 1) {
        //        data.push(d);
        //    }
        //});
        periods = $.map(data, function (d) {
            if (d.FiscalYear == thisFY() + 1)
            return d.FiscalPeriod;
        });
        //console.log(periods);

        budgets = $.map(data, function (d) {
            if (d.FiscalYear == thisFY() + 1)
                return d.BudgetValue;
        });
        //console.log(budgets);

        var chart = c3.generate({
            bindto: '#budgetchart',
            padding: {left:50},
            data: {
                x: 'x',
                columns: [
                   ['x'].concat(periods),
                   ['budget'].concat(budgets)
                ],
                names: {
                    budget: 'Budget',
                },
                types: {
                    budget: 'bar'
                },
                colors: {
                    budget: 'darkred'
                },
            },
            axis: {
                x: {
                    tick: {
                        culling: false,
                         format: function (x) { return getMonth(x); }
                    },
                    label: '2015/16 period'
                },
                y: {
                    label: 'GBP millions',
                    //tick: { format: d3.format(',d') },
                    tick: {
                        format: function (x) { return getMillions(x); }
                    },
                    min: 0,
                    padding: { bottom: 0 }
                }
            },
            legend: {
                position: 'inset',
                inset: {
                    anchor: 'top-right',
                    x: 20,
                    y: 50,
                    step: undefined
                }
            },
            tooltip: {
                format: {
                    title: function (d) { return 'Period ' + d + ' (' + getMonth(d) + ')'; },
                    value: function (value, ratio, id) {
                        //var format = id === 'budget' ? d3.format(',d');
                        var format = d3.format(',.0f');
                        return '&pound;' + format(value);
                    }
                }
            }
        });
    });
}

function drawBudgetChartCumulative(OUCode) {
    $.getJSON('http://ekd-web-03/MIAPI/MI/CurrentBudget/' + OUCode + '?type=json', function (data) {

        //var data = new Array();

        //pull out this FY only
        //json.for(function (d) {
        //    if (d.FiscalYear == thisFY() + 1) {
        //        data.push(d);
        //    }
        //});
        periods = $.map(data, function (d) {
            if (d.FiscalYear == thisFY() + 1)
                return d.FiscalPeriod;
        });
        //console.log(periods);

        budgets = $.map(data, function (d) {
            if (d.FiscalYear == thisFY() + 1)
                return d.BudgetValueCumulative;
        });
        //console.log(budgets);

        var chart = c3.generate({
            bindto: '#budgetlinechart',
            padding: { left: 50 },
            data: {
                x: 'x',
                columns: [
                   ['x'].concat(periods),
                   ['budget'].concat(budgets)
                ],
                names: {
                    budget: 'Budget',
                },
                types: {
                    budget: 'bar'
                },
                colors: {
                    budget: 'darkred'
                },
            },
            axis: {
                x: {
                    tick: {
                        culling: false,
                        format: function (x) { return getMonth(x); }
                    },
                    label: '2015/16 period'
                },
                y: {
                    label: 'GBP millions',
                    //tick: { format: d3.format(',d') },
                    tick: {
                        format: function (x) { return getMillions(x); }
                    },
                    min: 0,
                    padding: { bottom: 0 }
                }
            },
            legend: {
                position: 'inset',
                inset: {
                    anchor: 'top-right',
                    x: 20,
                    y: 50,
                    step: undefined
                }
            },
            tooltip: {
                format: {
                    title: function (d) { return 'Period ' + d + ' (' + getMonth(d) + ')'; },
                    value: function (value, ratio, id) {
                        //var format = id === 'budget' ? d3.format(',d');
                        var format = d3.format(',.0f');
                        return '&pound;' + format(value);
                    }
                }
            }
        });
    });
    document.getElementById("budgetchartNextFYlink").innerHTML = chartDataURL(OUCode);
}

function getMonth(p) {
    var month = ['-','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec','Jan','Feb','Mar'];
    return month[p];
}
function getMillions(p) {
    var format= d3.format(',d');
    return format(p / 1000000);
}

function fillTable(OUCode) {
    $.getJSON('http://ekd-web-03/MIAPI/MI/CurrentBudget/' + OUCode + '?type=json', function (data) {
        $('#table1')
            .TidyTable({
                columnTitles: ['Column A', 'Column B', 'Column C', 'Column D', 'Column E'],
                columnValues: data
            });
    }
    );
}

function chartDataURL(OUCode) {
    var dataURL = "http://reports/ReportServer?/DFID+Data+Central%2fMI+Reports%2fMI+Dashboard+Data&rc:Toolbar=false&rs:Command=Render&OUCode=" + OUCode + "&rs:format=HTML4.0";
    return "<a href='" + dataURL + "'>Get the raw data</a>";
}

