(function (global, factory) {
	typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports) :
	typeof define === 'function' && define.amd ? define(['exports'], factory) :
	(factory((global.viz = global.viz || {})));
    }(this, (function (exports) { 
    
        'use strict';

        var RScolors = ["#80633b","#0085a1", "#94948f", "#595959", "#f9e300", "#fecb00", "#fcc860", "#fc9e77", "#ff6d22", "#bd4f19",
        "#e21b23", "#a20234", "	#00b092", "	#4bdbc3", "#d4df4d",  "#9c9a00", "#005c42", "#64bf92", "#6fd4e4", "#3d7edb"];
        
        var Brown = RScolors[0];
        
        var RScolors3 = RScolors.slice(0,3).reverse();
        var RScolorsBubble = RScolors.slice(0,4).filter(function (x){ return x != Brown}).reverse();
        
        

        function fuldtid(csv){
            
            /* console.log("Csv: \n");
            console.log(csv);  */             
            
            //Set dimensions
            var margin = {top: 20, right: 20, bottom: 110, left: 40},
                width = 960 - margin.left - margin.right,
                height = 650 - margin.top - margin.bottom;
        
            //Set color-scheme 
            var color = d3.scaleOrdinal().range(RScolors);
            
            //Set scales and ranges
            var x_groups = d3.scaleBand().range([0, width]).padding(0.01);
            var x_categories = d3.scaleBand().padding(0.1);
            var y = d3.scaleLinear().range([height, 0]);
        
            //Append svg-object to div
            var svg = d3.select("#fuldtid").append("svg")
                .attr("width", width + margin.left + margin.right)
                .attr("height", height + margin.top + margin.bottom)
                .append("g")
                .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

                
            //Get data
            var ssv = d3.dsvFormat(";");
            var data = ssv.parse(csv);
            
            //console.log(JSON.stringify(data));
        
            data.forEach(function(d){
                //d.Year = + d.Year;
                d.Fuldtid = parseFloat((d.Fuldtid.replace(",",".")));
            });
        
            /* console.log("With numbers:\n");
            console.log(JSON.stringify(data)); */
        
        
            //Nest data by virk and gender
            var dataByVirkGender = d3.nest()
                .key(function (d) { return d.Virk; })
                .key(function (d) { return d.Gender; })
                .rollup(function (v) { return d3.sum(v, function (d) {return d.Fuldtid; }); })
                .entries(data);

            console.log("DataVirkByGender: " + JSON.stringify(dataByVirkGender));
                    
            //Sort data by sum of fuldtidsansatte pr. virksomhedsormåde
            dataByVirkGender.sort(function (a,b) {return ( b.values.map(function (d) {return d.value; }).reduce (function (x,y) {return x + y;}) 
            - (a.values.map(function (d) {return d.value; })).reduce (function (x,y) {return x + y;})  );  });
            
            //console.log("Sorted: " + JSON.stringify(dataByVirkGender));
                    
            
            //Domains
            //var groups = [...new Set(data.map(d => d.Virk))];
            var groups = [...new Set(dataByVirkGender.map(d => d.key))];
            console.log("Groups: " + groups);
            x_groups.domain(groups);
            
            //Get categories - exctract the keys from the first entry
            //var categories = dataByVirkGender[0].values.map(function (d) {return d.key; } );
            var categories  = [...new Set(data.map(d => d.Gender))];
            console.log("Categories: " + categories);
            x_categories.domain(categories).rangeRound([0, x_groups.bandwidth()]);
            
            
            var values = dataByVirkGender.map(d => d.values);
            //console.log("Values: " + JSON.stringify(values));

            var test = values.reduce(function (a,b) {return a.concat(b);});
            //console.log("Test: " + JSON.stringify(test));
            
            console.log("max fuldtid: " + d3.max(test.map(d => d.value)));

            y.domain([0, d3.max(test.map(d => d.value))]);

            
            
            
            
            // Create g's for groups (virk)
            var groups_g = svg.selectAll("group")
                .data(dataByVirkGender)
                .enter().append("g")
                .attr("class", function (d) {return 'group group-' + d.key;} )
                .attr("transform", function (d) {
                    return "translate(" + x_groups(d.key) + ",0)"; });
            
                    
            //Tooltips
            var tooltip = d3.select('#fuldtid')           
                .append('div')                            
                .attr('class', 'mytooltip');                

                tooltip.append('div')                        
                .attr('class', 'gender');                   

                tooltip.append('div')                        
                .attr('class', 'count');                   

                tooltip.append('div')                       
                .attr('class', 'percent');              
            
            //Append rects for all gender values for each virk
            var rects = groups_g.selectAll("rect")
                .data(function(d) { return d.values; })
                .enter()
                .append("rect")
                .attr("class", function (d) { return "category category-" + d.key})
                .attr("x", function(d) {return x_categories(d.key);})
                .attr("y", 0)
                //.attr("y", function (d) { return y(d.value); })
                .attr("width", x_categories.bandwidth())
                .attr("height", function (d) { return height - y(d.value); })
                .attr("fill", function(d) { return color(d.key); })
                .attr("data-legend",function(d) { return d.key});
            
                rects.transition()
                .duration(1000)
                .attr("y", function (d) { return y(d.value); });
        


                                                 // NEW    
        
            //  Add x axis
            var x_axis = svg.append("g")
                .attr("class", "axis")
                .attr("transform", "translate(0," + height +  ")")
                .call(d3.axisBottom(x_groups))
                .selectAll("text")
                //.attr("y", 0)
                //.attr("x", 9)
                /* .attr("dy", ".35em") */
                //.attr("transform", "rotate(90)")
                //.attr("transform",
                      // "translate(" + (width/2) + " ," + (height + margin.top + 20) + ")")
                
                //.style("text-anchor", "start")
                .style("text-anchor", "end")
                .attr("dx", "-.8em")
                .attr("dy", ".15em")
                .attr("transform", "rotate(-65)")
                .call(wrap, margin.bottom);
            
            // Add y axis
            var y_axis = svg.append("g")
                .attr("class","axis")
                .call(d3.axisLeft(y));

            // Add legends
            var legendRectSize = 18;                                  // NEW
            var legendSpacing = 4;                                    // NEW

            var legend = svg.selectAll('.legend')
                .data(color.domain())
                .enter()
                .append('g')
                .attr('class', 'legend')
                .attr('transform', function(d, i) {                     // NEW
                    var height = legendRectSize + legendSpacing;          // NEW
                    var offset =  height * color.domain().length / 2;     // NEW
                    var horz = width - legendRectSize*3;                       // NEW
                    var vert = i * height - offset;                       // NEW
                    return 'translate(' + horz + ',' + vert + ')';        // NEW
                  });

            legend.append('rect')
                .attr('width', legendRectSize)
                .attr('height', legendRectSize)
                .style('fill', color)
                .style('stroke', color);
            
            legend.append('text')
                .attr('x', legendRectSize + legendSpacing)
                .attr('y', legendRectSize - legendSpacing)
                .text(function(d) { return d; });        

            rects.on("mouseover", function(d) {           // NEW
                var total = d3.sum(data.map(function (d) {return d.Fuldtid}));
                var fuldtid = Math.round(d.value);
                var percent = Math.round(1000 * fuldtid / total) / 10;
                tooltip.select('.gender').html(d.key);
                tooltip.select('.count').html(fuldtid + ' fuldtidsansatte');
                tooltip.select('.percent').html(percent + '% af regionens fuldtidsansatte');
                tooltip.style('display', 'block');
                // NEW
                });
                
            rects.on('mousemove', function(d) {
                    tooltip.style('top', (d3.event.layerY + 10) + 'px')
                      .style('left', (d3.event.layerX + 10) + 'px');
                  });// NEW
                
            rects.on('mouseout', function(d) {            // NEW
                tooltip.style('display', 'none');                                  // NEW
                });        
            
            
        }

        function bubble(csv){
            
            //Set dimensions
            var margin = 20,
            width = 960, 
            height = 960,
            diameter = width;

            //Set color-scheme 
            var color = d3.scaleOrdinal().range(RScolorsBubble);

            //Append svg-object to div
            var svg = d3.select("#bubble").append("svg")
                .attr("width", width)
                .attr("height", height)

            var g = svg.append("g")
                .attr("transform", "translate(" + diameter / 2 + "," + diameter / 2 + ")");

            //Create pack    
            var pack = d3.pack()
                .size([diameter - margin, diameter - margin])
                .padding(2);

            //Get data
            var ssv = d3.dsvFormat(";");
            var data = ssv.parse(csv);
                    
            data.forEach(function(d){
                //d.Year = + d.Year;
                d.Fuldtid = parseFloat((d.Fuldtid.replace(",",".")));
            });
        
            /* console.log("Parsed data:\n");
            console.log(JSON.stringify(data)); */
        
        
            //Nest data by virk and gender
            var dataSummed = d3.nest()
                //.key(function (d) { return d.Year; })
                .key(function (d) { return d.Virk; })
                .key(function (d) { return d.Kat; })
                .key(function (d) { return d.Gender; })
                .rollup(function (v) { return d3.sum(v, function (d) {return d.Fuldtid; }); })
                .entries(data);

            console.log("DataSummed: " + JSON.stringify(dataSummed));
            
            

            //TEST
            /* var dataSummed = 
            {"name": "Region Sjælland", 
            "children": [
                {"name":"Sygehusapoteket",
                "children":[
                    {"name":"Administrion","antal":150},
                    {"name":"Apotekspersonale","antal":20}
                ]
                },
                {"name":"Direktionen",
                    "children":[
                        {"name":"Administration","antal":10},
                        {"name":"Syge,sundhed og plejepersonale","antal":15}
                ]
                },
                {"name":"Koncern HR",
                    "children":[
                    {"name":"Administration","antal":102}]}]}; */
            /* var stratified = d3.stratify()(dataSummed);
               // .sum(function (d) {return d.value ? 1 : 0;});   
               console.log("Stratified: ") 
               console.dir(stratified); */

            var root = d3.hierarchy({ values: dataSummed }, function (d) { return d.values })
                .sum(function (d) {return d.value;})
                .sort(function(a, b) { return b.value - a.value; });

            /* console.log("Root: ") 
            console.dir(root); */

            var focus = root,
            nodes = pack(root).descendants(),
            view;

            /* console.log("nodes: ");
            console.dir(nodes); */
        

            var circle = g.selectAll("circle")
                .data(nodes)
                .enter().append("circle")
                .attr("class", function(d) { return d.parent ? d.children ? "node" : "node node--leaf--" + d.data.key : "node node--root"; })
                .style("fill", function(d) { return d.children ? color(d.depth) : null; })
                .on("click", function(d) { if (focus !== d) zoom(d), d3.event.stopPropagation(); });

            var text = g.selectAll("text")
                .data(nodes)
                .enter().append("text")
                .attr("class", "label")
                .style("fill-opacity", function(d) { return d.parent === root ? 1 : 0; })
                .style("display", function(d) { return d.parent === root ? "inline" : "none"; })
                .text(function(d) { return d.data.key; });

            var node = g.selectAll("circle,text");

            svg
                //.style("background", color(-1))
                .on("click", function() { zoom(root); });

            zoomTo([root.x, root.y, root.r * 2 + margin]);

            function zoom(d) {
                var focus0 = focus; focus = d;

                var transition = d3.transition()
                    .duration(d3.event.altKey ? 7500 : 750)
                    .tween("zoom", function(d) {
                    var i = d3.interpolateZoom(view, [focus.x, focus.y, focus.r * 2 + margin]);
                    return function(t) { zoomTo(i(t)); };
                    });

                transition.selectAll("text")
                .filter(function(d) { return d.parent === focus || this.style.display === "inline"; })
                    .style("fill-opacity", function(d) { return d.parent === focus ? 1 : 0; })
                    .on("start", function(d) { if (d.parent === focus) this.style.display = "inline"; })
                    .on("end", function(d) { if (d.parent !== focus) this.style.display = "none"; });
            }

            function zoomTo(v) {
                var k = diameter / v[2]; view = v;
                node.attr("transform", function(d) { return "translate(" + (d.x - v[0]) * k + "," + (d.y - v[1]) * k + ")"; });
                circle.attr("r", function(d) { return d.r * k; });
        }



        }

        function antal(csv){
            
               /*  console.log("Csv: \n");
                console.log(csv);  */
                           
                
                //Set dimensions
                var margin = {top: 20, right: 20, bottom: 20, left: 150},
                    width = 960 - margin.left - margin.right,
                    height = 500 - margin.top - margin.bottom;
            
                //Set color-scheme 
                var color = d3.scaleOrdinal().range(RScolors3);
                
                //Set scales and ranges
                var x = d3.scaleLinear().range([0, width]);
                var y = d3.scaleBand().range([height, 0]).padding(0.1);
                
            
                //Append svg-object to div
                var svg = d3.select("#fuldtid-deltid").append("svg")
                    .attr("width", width + margin.left + margin.right)
                    .attr("height", height + margin.top + margin.bottom)
                    .append("g")
                    .attr("transform", "translate(" + margin.left + "," + margin.top + ")");
    
                    
                //Get data
                var ssv = d3.dsvFormat(";");
                var data = ssv.parse(csv);
                
                //console.log(JSON.stringify(data));
            
                data.forEach(function(d){
                    //d.Year = + d.Year;
                    d.Antal = + d.Antal;
                });
            
                /* console.log("Parsed data:\n");
                console.log(JSON.stringify(data)); */
            
            
                //Nest data by virk and gender
                var dataSummed = d3.nest()
                    .key(function (d) { return d.Arbejdstid; })
                    .rollup(function (v) { return d3.sum(v, function (d) {return d.Antal; }); })
                    .entries(data);
    
                console.log("DataSummed: " + JSON.stringify(dataSummed));

                //Sort Datasummed
                dataSummed.sort(function (a,b) {return a.value - b.value;});
                console.log("DataSummed sorted: " + JSON.stringify(dataSummed));        
                                          
                //Domains
                                
                //Get categories - exctract the keys from the first entry
                //var categories = dataByVirkGender[0].values.map(function (d) {return d.key; } );
                
                y.domain(dataSummed.map(function (d) {return d.key; } ));
                x.domain([0, d3.max(dataSummed, function (d) {return d.value;})]);
                
                //console.log("Max antal: " + d3.max(dataSummed, function (d) {return d.value;}));
    
                
    
                                      
                
                
                var rects = svg.selectAll("rect")
                    .data(dataSummed)
                    .enter()
                    .append("rect")
                    .attr("y", function(d) {return y(d.key);})
                    .attr("x", 0)
                    .attr("height", y.bandwidth())
                    .attr("width", function (d) { return x(d.value); })
                    .attr("fill", function(d) { return color(d.key); });
            
                //  Add x axis
                var y_axis = svg.append("g")
                    .attr("class", "axis")
                    .call(d3.axisLeft(y));
                   
                  
                
                
                // Add y axis
                var x_axis = svg.append("g")
                    .attr("transform", "translate(0," + height + ")")
                    .attr("class","axis")
                    .call(d3.axisBottom(x)); 
                
            }
        
            //Function to wrap axis label text
            function wrap(text, width) {
                text.each(function() {
                  var text = d3.select(this),
                      words = text.text().split(/\s+/).reverse(),
                      word,
                      line = [],
                      lineNumber = 0,
                      lineHeight = 1.1, // ems
                      y = text.attr("y"),
                      dy = parseFloat(text.attr("dy")),
                      tspan = text.text(null).append("tspan").attr("x", 0).attr("y", y).attr("dy", dy + "em");
                  while (word = words.pop()) {
                    line.push(word);
                    tspan.text(line.join(" "));
                    if (tspan.node().getComputedTextLength() > width) {
                      line.pop();
                      tspan.text(line.join(" "));
                      line = [word];
                      tspan = text.append("tspan").attr("x", 0).attr("y", y).attr("dy", ++lineNumber * lineHeight + dy + "em").text(word);
                    }
                  }
                });
              }
                     

        exports.fuldtid = fuldtid;
        exports.antal = antal;
        exports.bubble = bubble;
        
        
        Object.defineProperty(exports, '__esModule', { value: true });
    
    })));


