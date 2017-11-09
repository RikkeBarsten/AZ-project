// Write your Javascript code.
(function()
{

    
    var csv = "Year;Virk;Gender;Fuldtid\n2016;Apo-Sygehusapoteket;Kvinde;152,23\n2016;Apo-Sygehusapoteket;Mand;17,24\n2016;Direktionen;Mand;3\n"


            console.log("Csv: \n");
            console.log(csv); 

        //Set dimensions
            var margin = {top: 20, right: 20, bottom: 70, left: 40},
                width = 960 - margin.left - margin.right,
                height = 500 - margin.top - margin.bottom;
    
        //Set ranges
        var x = d3.scaleBand().range([0, width]).padding(0.1);
        var y = d3.scaleLinear().range([height, 0]);

        

        //Append svg-object to div
        var svg = d3.select("#Fuldtid").append("svg")
            .attr("width", width + margin.left + margin.right)
            .attr("height", height + margin.top + margin.bottom)
            .append("g")
            .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

        //Get data

        var ssv = d3.dsvFormat(";");
        var data = ssv.parse(csv);
        console.log(JSON.stringify(data));

        data.forEach(function(d){
            //d.Year = + d.Year;
            d.Fuldtid = parseFloat((d.Fuldtid.replace(",",".")));
        });

        console.log("With numbers:\n");
        console.log(JSON.stringify(data));

        //Scales
        x.domain(data.map(function(d){ return d.Year; }));
        y.domain([0, d3.max(data, function(d){ return d.Fuldtid; })])


        //Append rects

        svg.selectAll(".bar")
            .data(data)
            .enter().append("rect")
            .attr("class", "bar")
            .attr("x", function(d) {return x(d.Year);})
            .attr("width", x.bandwidth)
            .attr("y", function (d) { return y(d.Fuldtid); })
            .attr("height", function (d) { return height - y(d.Fuldtid); });


        // Add x axis
        svg.append("g")
            .attr("transform", "translate(0," + height + ")")
            .call(d3.axisBottom(x));


        // Add y axis

        svg.append("g").call(d3.axisLeft(y));




        
        
    

})()
