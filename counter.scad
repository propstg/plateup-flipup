scale([0.01, 0.01, 0.01])
//referenceCube();
//counterBase();
//counterOverBase();
//counterOverBaseRim();

//liftMechanism();

//counterOpen();
//armOpen();
//counterOpenRim();

//counterClosed();
//armClosed();
counterClosedRim();

module counterBase() {
    color("white")
    translate([-50, 30, 0])
    cube([100, 20, 40]);
}

module counterOverBase() {
    color("brown")
    translate([-50, 30, 40])
    cube([100, 20, 10]);
}

module counterOverBaseRim() {
    difference() {
        color("green")
        translate([-50.05, 30-0.1, 40])
        cube([100.1, 20.2, 9.9]);
        
        translate([0, 0, -0.01])
        counterOverBase();
    }
}

module counterOpen() {
    color("brown")
    translate([-50, 20, 50])
    cube([100, 10, 80]);
}

module counterOpenRim() {
    difference() {
        color("green")
        translate([-50.05, 20.05, 49.95])
        cube([100.1, 9.9, 80.1]);
        
        counterOpen();
    }
}

module liftMechanism() {
    color("black")
    translate([-50, 18, 22])
    cube([100, 12, 15]);
}

module armOpen() {
    color("silver")
    translate([-5, 20, 35])
    rotate([90, 0, 0])
    cube([10, 50, 3]);
}

module armClosed() {
    color("silver")
    translate([-5, -30, 38])
    rotate([-5, 0, 0])
    cube([10, 50, 3]);
}

module counterClosed() {
    color("brown")
    translate([-50, -50, 40])
    cube([100, 80, 10]);
}

module counterClosedRim() {
    difference() {
        color("green")
        translate([-50.05, -50.05, 40])
        cube([100.1, 80.1, 9.9]);
        
        translate([0, 0, -0.01])
        counterClosed();
    }
}

module referenceCube() {
    translate([-50, -200, 0])
    cube([100, 100, 100]);
}