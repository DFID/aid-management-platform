$(document).ready(function () {

    //build the margins of the container for the graph
    var margin = { top: 20, right: 20, bottom: 40, left: 100 },
        width = 900 - margin.left - margin.right,
        height = 300 - margin.top - margin.bottom;

    var x = d3.scale.ordinal()
        .rangeRoundBands([0, width], .1, 0);

    var y = d3.scale.linear().range([height, 0]);

    var xAxis = d3.svg.axis().scale(x)
        .orient("bottom").ticks(5);

    var yAxis = d3.svg.axis().scale(y)
        .orient("left").ticks(5);

    var area = d3.svg.area()
        .x(function (d) { return x(d.period); })
        .y0(height)
        .y1(function (d) { return y(d.Spend); });

    var areaForecast = d3.svg.area()
                    .x(function (d) { return x(d.period); })
                    .y0(height)
                    .y1(function (d) { return y(d.RemainingForecast); });

    var forecastline = d3.svg.line()
                  .x(function (d) { return x(d.period); })
                  .y(function (d) { return y(d.RemainingForecast); });

    var valueline = d3.svg.line()
        .x(function (d) { return x(d.period); })
        .y(function (d) { return y(d.Spend); });

    var budgetline = d3.svg.line()
         .x(function (d) { return x(d.period); })
         .y(function (d) { return y(d.Revised_Budget); });

    var div = d3.select("div.ProjectPeriodGraph")
        .append("div")
            .attr("class", "tooltip")
            .style("opacity", 0);

    var svg = d3.select("div.ProjectPeriodGraph")
        .append("svg")
            .attr("width", width + margin.left + margin.right)
            .attr("height", height + margin.top + margin.bottom)
        .append("g")
            .attr("transform", "translate(" + margin.left + "," + margin.top + ")");


    //Find ProjectID on the page
    var ProjectID = document.getElementById("ProjectID").innerHTML;
    
    // Get the data if using JSON request.
    d3.json('http://'+ProjectID+'?type=json', function (error, data) {

        x.domain(data.map(function (d) { return d.period; }));
        y.domain([0, d3.max(data, function (d) { return d.Revised_Budget; })]);

        svg.append("path")      // Add the forecast line path.
               .attr("d", forecastline(data))
          .attr("class", "path3")
             .attr("visibility", "hidden");

        //add the area for spend
        svg.append("path")
            .datum(data)
            .attr("class", "area")
            .attr("d", area);

        // Add the area for forecast
        svg.append("path")
       .datum(data)
       .attr("class", "areaForecast")
        .attr("visibility", "hidden")
       .attr("d", areaForecast);

        svg.append("path")      // Add the valueline path.
            .attr("d", valueline(data));

        svg.append("path")      // Add the budgetline path.
             .attr("d", budgetline(data))
        .attr("id", "budgetline")
        .attr("class", "path2");

        svg.append("g")         // Add the X Axis
            .attr("class", "x axis")
            .attr("transform", "translate(-24," + height + ")")
            .call(xAxis);

        svg.append("g")         // Add the Y Axis
            .attr("class", "y axis")
            .call(yAxis);

        //add hover-over tooltips
        svg.selectAll("dot")
            .data(data)
        .enter().append("circle")
            .attr("r", 5)
            .attr("cx", function (d) { return x(d.period); })
            .attr("cy", function (d) { return y(d.Spend); })
            .style("opacity", 0)
            .on("mouseover", function (d) {
                div.transition()
                    .duration(200)
                    .style("opacity", .9);
                div.html(" Spend:<br/>£" + (d.Spend))
                    .style("left", (d3.event.pageX) + "px")
                    .style("top", (d3.event.pageY - 28) + "px");
            })
            .on("mouseout", function (d) {
                div.transition()
                    .duration(500)
                    .style("opacity", 0);
            });
    });
});